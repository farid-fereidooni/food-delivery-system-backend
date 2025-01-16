using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Restaurants;

public record RestaurantOwnerCreatedDenormalizationEvent(Guid Id) : Event;
