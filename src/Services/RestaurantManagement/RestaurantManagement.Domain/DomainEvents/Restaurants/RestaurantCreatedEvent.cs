using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Enums;

namespace RestaurantManagement.Domain.DomainEvents.Restaurants;

public record RestaurantCreatedEvent(
    Guid Id,
    Guid OwnerId,
    RestaurantStatus Status,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IDomainEvent;
