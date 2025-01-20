using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.FoodTypes;

public record FoodTypeRemovedDenormalizationEvent : Event
{
    public required Guid Id { get; init; }
}
