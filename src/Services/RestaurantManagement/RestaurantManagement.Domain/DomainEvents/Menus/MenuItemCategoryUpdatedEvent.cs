using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemCategoryUpdatedEvent(Guid Id, Guid MenuId, Guid RestaurantId, Guid CategoryId) : IDomainEvent;
