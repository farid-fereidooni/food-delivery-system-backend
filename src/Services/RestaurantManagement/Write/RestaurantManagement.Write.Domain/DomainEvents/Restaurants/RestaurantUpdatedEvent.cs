using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Restaurants;

public record RestaurantUpdatedEvent(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IDomainEvent;
