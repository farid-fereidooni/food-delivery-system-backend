using EventBus.Core;
using RestaurantManagement.Read.Domain.Enums;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;

public record RestaurantCreatedDenormalizationEvent(
    Guid Id,
    Guid OwnerId,
    string Name,
    RestaurantStatus Status,
    string Street,
    string City,
    string State,
    string ZipCode) : Event;
