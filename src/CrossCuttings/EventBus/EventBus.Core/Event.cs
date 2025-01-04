namespace EventBus.Core;

public abstract record Event
{
    public Guid EventId { get; } = Guid.NewGuid();
    public string EventName => this.GetType().Name;
    public DateTime EventCreatedAt { get; } = DateTime.UtcNow;
}
