using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Restaurants;

public record RestaurantDeactivatedDenormalizationEvent(Guid Id) : Event;
