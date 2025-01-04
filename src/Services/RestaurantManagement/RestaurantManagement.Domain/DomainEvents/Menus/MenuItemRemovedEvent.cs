using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemRemovedEvent(Guid Id, Guid MenuId, Guid RestaurantId) : IDomainEvent;
