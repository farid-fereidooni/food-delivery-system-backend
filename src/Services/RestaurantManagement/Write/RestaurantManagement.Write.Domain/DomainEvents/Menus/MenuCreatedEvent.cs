using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuCreatedEvent(Guid Id, Guid RestaurantId) : IDomainEvent;
