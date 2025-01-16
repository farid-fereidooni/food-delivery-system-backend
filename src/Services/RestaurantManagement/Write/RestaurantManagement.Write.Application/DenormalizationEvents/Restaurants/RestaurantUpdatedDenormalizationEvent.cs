using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Restaurants;

public record RestaurantUpdatedDenormalizationEvent(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : Event;
