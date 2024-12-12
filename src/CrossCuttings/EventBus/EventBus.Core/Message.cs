namespace EventBus.Core;

public record Message
{
    public required string EventName { get; init; }
    public required string Content { get; init; }
    public required string Topic { get; init; }
}
