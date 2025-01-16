using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Restaurants;

public record RestaurantDeactivatedEvent(Guid Id) : IDomainEvent;
