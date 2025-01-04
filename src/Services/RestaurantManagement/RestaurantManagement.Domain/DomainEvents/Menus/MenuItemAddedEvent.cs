using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Menus;

public record MenuItemAddedEvent(
    Guid Id,
    Guid MenuId,
    Guid RestaurantId,
    Guid CategoryId,
    string Name,
    decimal Price,
    string? Description,
    uint Stock,
    ICollection<Guid> FoodTypeIds) : IDomainEvent;
