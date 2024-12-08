namespace EventBus.Models;

internal class EventBusConfiguration
{
    public required string Host { get; set; }
    public required Subscriptions Subscriptions { get; set; }
}
