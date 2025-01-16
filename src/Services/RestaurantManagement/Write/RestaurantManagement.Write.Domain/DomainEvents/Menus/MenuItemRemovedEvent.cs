using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuItemRemovedEvent(Guid Id, Guid MenuId, Guid RestaurantId) : IDomainEvent;
