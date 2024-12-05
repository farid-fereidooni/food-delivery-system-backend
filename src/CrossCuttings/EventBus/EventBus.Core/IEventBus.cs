namespace EventBus.Core;

public interface IEventBus
{
    Task PublishAsync<TEvent>(string topic, TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    public Task StartConsumeAsync();
}
