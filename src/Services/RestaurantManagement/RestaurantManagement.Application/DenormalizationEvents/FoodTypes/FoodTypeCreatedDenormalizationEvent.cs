using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.FoodTypes;

public record FoodTypeCreatedDenormalizationEvent : Event
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
