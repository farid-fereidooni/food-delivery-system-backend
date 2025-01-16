using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Menus;

public record MenuItemFoodTypesUpdatedEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, ICollection<Guid> FoodTypes) : IDomainEvent;
