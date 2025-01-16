using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventBus.Core;
using EventBus.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus;

internal class RabbitMqEventBus : IEventBus, IDisposable
{
    private readonly EventBusConfiguration _eventBusConfiguration;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly IServiceProvider _services;
    private IConnection? _connection = null;

    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly Dictionary<string, IChannel> _consumerChannels = new();


    [MemberNotNullWhen(true, nameof(_connection))]
    private bool IsConnected => _connection is not null && _connection.IsOpen;

    public RabbitMqEventBus(
        EventBusConfiguration configuration,
        ILogger<RabbitMqEventBus> logger,
        IServiceProvider services)
    {
        _eventBusConfiguration = configuration;
        _logger = logger;
        _services = services;
    }

    public async Task PublishAsync(string topic, Event @event, CancellationToken cancellationToken = default)
    {
        var eventName = NameOfEvent(@event);
        var message = JsonSerializer.SerializeToUtf8Bytes(@event, Constants.JsonSerializerOptions);

        await PublishAsyncCore(topic, eventName, message, cancellationToken);
    }

    public async Task PublishAsync(Message message, CancellationToken cancellationToken = default)
    {
        var body = Encoding.UTF8.GetBytes(message.Content);
        await PublishAsyncCore(message.Topic, message.EventName, body, cancellationToken);
    }

    private async Task PublishAsyncCore(
        string topic, string eventName, byte[] content, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
            await Connect(cancellationToken);

        await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await DeclareExchange(channel, topic, cancellationToken);

        await channel.BasicPublishAsync(topic, eventName, body: content, cancellationToken: cancellationToken);
    }

    public async Task StartConsumeAsync(CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
            await Connect(cancellationToken);

        foreach (var exchange in _eventBusConfiguration.Subscriptions)
        {
            var exchangeChannel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await DeclareExchange(exchangeChannel, exchange.Key, cancellationToken);

            _consumerChannels[exchange.Key] = exchangeChannel;

            await InitializeQueueAndConsume(exchange.Key, exchangeChannel, exchange.Value, cancellationToken);
        }
    }

    private async Task DeclareExchange(
        IChannel channel, string exchangeName, CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: cancellationToken);
    }

    private async Task InitializeQueueAndConsume(
        string exchange, IChannel channel, Queues queues, CancellationToken cancellationToken)
    {
        foreach (var queue in queues)
        {
            await channel.QueueDeclareAsync(
                queue: queue.Key,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await BindQueues(exchange, channel, queue.Key, queue.Value, cancellationToken);
            await ConsumeAsync(channel, queue.Key, cancellationToken);
        }
    }

    private async Task BindQueues(
        string exchange, IChannel channel, string queueName, Events events, CancellationToken cancellationToken)
    {;
        foreach (var @event in events)
            await channel.QueueBindAsync(
                queue: queueName, exchange: exchange, routingKey: @event.Key, cancellationToken: cancellationToken);
    }

    private async Task ConsumeAsync(IChannel channel, string queueName, CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += EventReceived;

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

    private async Task EventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;

        var eventType = _eventBusConfiguration.Subscriptions[eventArgs.Exchange].GetEvent(eventName);
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        var @event = JsonSerializer.Deserialize(message, eventType, Constants.JsonSerializerOptions);

        if (@event is null)
            throw new Exception("Invalid configuration of subscription in the eventbus");

        using var scope = _services.CreateScope();
        var handlers = scope.ServiceProvider.GetKeyedServices<IEventHandlerProxy>(eventName);

        var consumerChannel = _consumerChannels[eventArgs.Exchange];
        try
        {
            await Task.WhenAll(handlers.Select(handler => handler.HandleAsync(@event, eventArgs.CancellationToken)));
            await TryAck(consumerChannel, eventArgs.DeliveryTag, eventArgs.CancellationToken);
        }
        catch (EventIgnoredException ex)
        {
            _logger.LogWarning(ex, "Event {eventName} was ignored", eventName);
            await consumerChannel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Event {eventName} was not handled. Event nacked.", eventName);
            await consumerChannel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
        }
    }

    private async Task TryAck(IChannel channel, ulong deliveryTag, CancellationToken cancellationToken = default)
    {
        try
        {
            await channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Basic Ack Failed. The message is being Requeued");
            await channel.BasicNackAsync(
                deliveryTag, multiple: false, requeue: true, cancellationToken: cancellationToken);
        }
    }

    [MemberNotNull(nameof(_connection))]
    private async Task Connect(CancellationToken cancellationToken = default)
    {
        await _connectionLock.WaitAsync(cancellationToken);

        var connectionFactory = new ConnectionFactory
        {
            HostName = _eventBusConfiguration.Host,
            AutomaticRecoveryEnabled = true,
        };

        await ConnectPolicy().ExecuteAsync(async () =>
        {
            _connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        });

        _connectionLock.Release();
    }

    private string NameOfEvent<TEvent>(TEvent @event) where TEvent : Event
    {
        return @event.EventName;
    }

    private AsyncPolicy ConnectPolicy() =>
        Policy
            .Handle<SocketException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(5),
                onRetry: (exception, retry, time) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Exception \"{Message}\" occured on connecting to event bus. retry attempt {retry}",
                        exception.Message,
                        retry);
                });

    public void Dispose()
    {
        _connection?.Dispose();
        foreach (var channel in _consumerChannels.Values)
            channel.Dispose();
    }
}
