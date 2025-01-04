using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Restaurants;

public record RestaurantActivatedDenormalizationEvent(Guid Id) : Event;
