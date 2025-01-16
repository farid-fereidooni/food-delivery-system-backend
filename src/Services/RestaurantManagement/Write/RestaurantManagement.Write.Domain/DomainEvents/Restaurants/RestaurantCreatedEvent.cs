using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Enums;

namespace RestaurantManagement.Write.Domain.DomainEvents.Restaurants;

public record RestaurantCreatedEvent(
    Guid Id,
    Guid OwnerId,
    RestaurantStatus Status,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IDomainEvent;
