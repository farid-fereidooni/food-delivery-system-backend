namespace FileManager.Core.Models;

public class EventBusConfiguration
{
    public required string Host { get; set; }
    public required string IntegrationEventBroker { get; set; }
    public required string IntegrationEventQueue { get; set; }
}
