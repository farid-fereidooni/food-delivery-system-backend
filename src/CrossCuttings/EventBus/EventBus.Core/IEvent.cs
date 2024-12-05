namespace EventBus.Core;

public abstract record IEvent(Guid Id, DateTime CreatedAt);
