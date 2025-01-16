using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Restaurants;

public record RestaurantOwnerCreatedEvent(Guid Id) : IDomainEvent;
