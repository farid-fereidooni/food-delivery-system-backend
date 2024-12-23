namespace EventBus.Core;

public interface IEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

public interface IEventHandlerProxy
{
    Task HandleAsync(object @event, CancellationToken cancellationToken = default);
}

public class EventHandlerProxy<TEvent> : IEventHandler<TEvent>, IEventHandlerProxy where TEvent : Event
{
    private readonly IEventHandler<TEvent> _innerHandler;

    public EventHandlerProxy(IEventHandler<TEvent> innerHandler)
    {
        _innerHandler = innerHandler;
    }

    Task IEventHandler<TEvent>.HandleAsync(TEvent @event, CancellationToken cancellationToken)
    {
        return _innerHandler.HandleAsync(@event, cancellationToken);
    }

    public Task HandleAsync(object @event, CancellationToken cancellationToken = default)
    {
        return _innerHandler.HandleAsync((TEvent)@event, cancellationToken);
    }
}
