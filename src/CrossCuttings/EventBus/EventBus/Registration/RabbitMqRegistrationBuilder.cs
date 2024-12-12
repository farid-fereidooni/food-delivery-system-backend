using EventBus.Core;
using EventBus.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Registration;

public interface IRabbitMqRegistrationQueueBind
{
    IRabbitMqRegistrationQueueBind AddEventLogService<TUnitOfWork>() where TUnitOfWork : IEventLogUnitOfWork;
    IRabbitMqRegistrationHandlerBind<TEvent> AddSubscription<TEvent>(string topic, string queueName)
        where TEvent : IEvent;
}

public interface IRabbitMqRegistrationHandlerBind<TEvent> where TEvent : IEvent
{
    IRabbitMqRegistrationHandlerBind<TEvent> AddHandler<TEventHandler>()
        where TEventHandler : class, IEventHandler<TEvent>;
}


internal class RabbitMqRegistrationBuilder : IRabbitMqRegistrationQueueBind
{
    private readonly IServiceCollection _serviceCollection;
    public Subscriptions Subscriptions { get; internal set; } = new();


    public RabbitMqRegistrationBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IRabbitMqRegistrationQueueBind AddEventLogService<TUnitOfWork>() where TUnitOfWork : IEventLogUnitOfWork
    {
        _serviceCollection.AddScoped<IEventLogUnitOfWork>(s => s.GetRequiredService<TUnitOfWork>());
        return this;
    }

    public IRabbitMqRegistrationHandlerBind<TEvent> AddSubscription<TEvent>(string topic, string queueName)
        where TEvent : IEvent
    {
        Subscriptions.AddSubscription<TEvent>(topic, queueName);
        return new RabbitMqRegistrationHandlerBuilder<TEvent>(_serviceCollection, Subscriptions);
    }
}

internal class RabbitMqRegistrationHandlerBuilder<TEvent>
    : RabbitMqRegistrationBuilder, IRabbitMqRegistrationHandlerBind<TEvent>
    where TEvent : IEvent
{
    private readonly IServiceCollection _serviceCollection;

    public RabbitMqRegistrationHandlerBuilder(
        IServiceCollection serviceCollection, Subscriptions subscriptions)
        : base(serviceCollection)
    {
        Subscriptions = subscriptions;
        _serviceCollection = serviceCollection;
    }

    public IRabbitMqRegistrationHandlerBind<TEvent> AddHandler<TEventHandler>()
        where TEventHandler : class, IEventHandler<TEvent>
    {
        var key = typeof(TEvent).Name;

        _serviceCollection.AddScoped<IEventHandler<TEvent>, TEventHandler>();
        _serviceCollection.AddKeyedScoped<IEventHandlerProxy, EventHandlerProxy<TEvent>>(key);

        return this;
    }
}
