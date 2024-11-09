using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Core.Domain.DomainEvents;

public record RestaurantCreatedEvent(Guid RestaurantId) : IDomainEvent;
