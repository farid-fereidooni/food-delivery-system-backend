using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;

public record RestaurantOwnerCreatedDenormalizationEvent(Guid Id) : Event;
