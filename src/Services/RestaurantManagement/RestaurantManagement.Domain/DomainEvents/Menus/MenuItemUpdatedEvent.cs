using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemUpdatedEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, string Name, decimal Price, string? Description) : IDomainEvent;
