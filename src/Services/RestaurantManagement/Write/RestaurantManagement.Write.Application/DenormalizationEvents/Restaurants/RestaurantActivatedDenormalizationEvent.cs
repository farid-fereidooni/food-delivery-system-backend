using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Restaurants;

public record RestaurantActivatedDenormalizationEvent(Guid Id) : Event;
