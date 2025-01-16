using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Menus;

public record MenuItemAddedDenormalizationEvent(
    Guid Id,
    Guid MenuId,
    Guid RestaurantId,
    Guid CategoryId,
    string Name,
    decimal Price,
    string? Description,
    uint Stock,
    ICollection<Guid> FoodTypeIds) : Event;
