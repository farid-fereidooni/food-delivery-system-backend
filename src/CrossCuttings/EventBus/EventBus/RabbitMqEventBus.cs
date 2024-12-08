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

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    [MemberNotNullWhen(true, nameof(_connection))]
    private bool IsConnected => _connection is not null && _connection.IsOpen;

    public RabbitMqEventBus(
        IOptions<EventBusConfiguration> eventBusConfiguration,
        EventBusConfiguration configuration,
        ILogger<RabbitMqEventBus> logger,
        IServiceProvider services)
    {
        _eventBusConfiguration = configuration;
        _logger = logger;
        _services = services;
        _eventBusConfiguration = eventBusConfiguration.Value;

        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task PublishAsync<TEvent>(
        string topic, TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        if (!IsConnected)
            await Connect(cancellationToken);

        var eventName = NameOfEvent(@event);

        await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            topic, type: ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: cancellationToken);

        var message = JsonSerializer.SerializeToUtf8Bytes(@event, _jsonSerializerOptions);
        await channel.BasicPublishAsync(topic, eventName, body: message, cancellationToken: cancellationToken);
    }

    public async Task StartConsumeAsync()
    {
        if (!IsConnected)
            await Connect();

        foreach (var exchange in _eventBusConfiguration.Subscriptions)
        {
            var exchangeChannel = await _connection.CreateChannelAsync();
            _consumerChannels[exchange.Key] = exchangeChannel;

            await InitializeQueueAndConsume(exchange.Key, exchangeChannel, exchange.Value);
        }
    }

    private async Task InitializeQueueAndConsume(string exchange, IChannel channel, Queues queues)
    {
        foreach (var queue in queues)
        {
            await channel.QueueDeclareAsync(
                queue: queue.Key,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await BindQueues(exchange, channel, queue.Key, queue.Value);
            await ConsumeAsync(channel, queue.Key);
        }
    }

    private async Task BindQueues(string exchange, IChannel channel, string queueName, Events events)
    {;
        foreach (var @event in events)
            await channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: @event.Key);
    }

    private async Task ConsumeAsync(IChannel channel, string queueName)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += EventReceived;

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }

    private async Task EventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;

        var eventType = _eventBusConfiguration.Subscriptions[eventArgs.Exchange].GetEvent(eventName);
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        var @event = JsonSerializer.Deserialize(message, eventType, _jsonSerializerOptions);

        if (@event is null)
            throw new Exception("Invalid configuration of subscription in the eventbus");

        using var scope = _services.CreateScope();
        var handlers = scope.ServiceProvider.GetKeyedServices<IEventHandlerProxy>(eventName);

        await Task.WhenAll(handlers.Select(handler => handler.HandleAsync(@event)));
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

    private string NameOfEvent<TEvent>(TEvent @event) where TEvent : IEvent
    {
        return @event.GetType().Name;
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
