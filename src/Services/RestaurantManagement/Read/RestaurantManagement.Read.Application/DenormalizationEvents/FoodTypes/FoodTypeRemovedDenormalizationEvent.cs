using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.FoodTypes;

public record FoodTypeRemovedDenormalizationEvent : Event
{
    public required Guid Id { get; init; }
}
