using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Restaurants;

public record RestaurantOwnerCreatedEvent(Guid Id) : IDomainEvent;
