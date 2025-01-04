using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Restaurants;

public record RestaurantOwnerCreatedDenormalizationEvent(Guid Id) : Event;
