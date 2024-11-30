using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents;

public record RestaurantCreatedEvent(Guid RestaurantId) : IDomainEvent;
