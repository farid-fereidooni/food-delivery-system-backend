using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemFoodTypesUpdatedEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, ICollection<Guid> FoodTypes) : IDomainEvent;
