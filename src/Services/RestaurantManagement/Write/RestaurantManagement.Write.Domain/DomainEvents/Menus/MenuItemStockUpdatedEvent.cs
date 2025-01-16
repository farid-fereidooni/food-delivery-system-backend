using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuItemStockUpdatedEvent(Guid Id, Guid MenuId, Guid RestaurantId, uint Stock) : IDomainEvent;
