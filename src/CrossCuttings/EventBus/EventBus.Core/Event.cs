namespace EventBus.Core;

public abstract record Event
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime EventCreatedAt { get; } = DateTime.UtcNow;
}
