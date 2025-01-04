using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Restaurants;

public record RestaurantDeactivatedEvent(Guid Id) : IDomainEvent;
