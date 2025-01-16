using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuItemUpdatedEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, string Name, decimal Price, string? Description) : IDomainEvent;
