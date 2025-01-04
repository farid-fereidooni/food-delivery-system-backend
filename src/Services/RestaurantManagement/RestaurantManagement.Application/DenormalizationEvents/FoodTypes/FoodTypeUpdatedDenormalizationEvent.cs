using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.FoodTypes;

public record FoodTypeUpdatedDenormalizationEvent : Event
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
