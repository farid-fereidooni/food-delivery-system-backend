using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuItemCategoryUpdatedEvent(Guid Id, Guid MenuId, Guid RestaurantId, Guid CategoryId) : IDomainEvent;
