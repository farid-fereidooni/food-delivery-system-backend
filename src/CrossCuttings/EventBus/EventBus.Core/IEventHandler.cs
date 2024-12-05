namespace EventBus.Core;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}

public interface IEventHandlerProxy
{
    Task HandleAsync(object @event);
}

public class EventHandlerProxy<TEvent> : IEventHandler<TEvent>, IEventHandlerProxy where TEvent : IEvent
{
    private readonly IEventHandler<TEvent> _innerHandler;

    public EventHandlerProxy(IEventHandler<TEvent> innerHandler)
    {
        _innerHandler = innerHandler;
    }

    Task IEventHandler<TEvent>.HandleAsync(TEvent @event)
    {
        return _innerHandler.HandleAsync(@event);
    }

    Task IEventHandlerProxy.HandleAsync(object @event)
    {
        return _innerHandler.HandleAsync((TEvent)@event);
    }
}
