using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Restaurants;

public record RestaurantUpdatedEvent(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IDomainEvent;
