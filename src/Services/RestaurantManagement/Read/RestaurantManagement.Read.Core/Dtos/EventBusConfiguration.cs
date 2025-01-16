namespace RestaurantManagement.Read.Domain.Dtos;

public class EventBusConfiguration
{
    public required string Host { get; set; }
    public required string IntegrationEventBroker { get; set; }
    public required string IntegrationEventQueue { get; set; }
    public required string DenormalizationBroker { get; set; }
    public required string DenormalizationQueue { get; set; }
}
