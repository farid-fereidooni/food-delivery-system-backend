using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;

public record RestaurantActivatedDenormalizationEvent(Guid Id) : Event;
