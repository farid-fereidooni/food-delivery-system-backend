using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuCreatedEvent(Guid Id, Guid RestaurantId) : IDomainEvent;
