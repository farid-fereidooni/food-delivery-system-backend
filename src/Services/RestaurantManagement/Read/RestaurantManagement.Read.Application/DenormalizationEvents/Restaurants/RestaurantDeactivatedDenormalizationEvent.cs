using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;

public record RestaurantDeactivatedDenormalizationEvent(Guid Id) : Event;
