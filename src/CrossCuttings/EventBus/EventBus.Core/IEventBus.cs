namespace EventBus.Core;

public interface IEventBus
{
    Task PublishAsync(string topic, Event @event, CancellationToken cancellationToken = default);
    Task PublishAsync(Message message, CancellationToken cancellationToken = default);
    public Task StartConsumeAsync(CancellationToken cancellationToken = default);
}
