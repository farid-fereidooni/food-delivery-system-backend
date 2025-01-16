using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.FoodTypes;

public record FoodTypeCreatedDenormalizationEvent : Event
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
