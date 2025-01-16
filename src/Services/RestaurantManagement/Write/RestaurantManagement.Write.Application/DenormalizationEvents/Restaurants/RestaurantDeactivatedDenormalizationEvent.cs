using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Restaurants;

public record RestaurantDeactivatedDenormalizationEvent(Guid Id) : Event;
