using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemStockUpdatedEvent(Guid Id, Guid MenuId, Guid RestaurantId, uint Stock) : IDomainEvent;
