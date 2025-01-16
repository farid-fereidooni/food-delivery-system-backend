using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Menus;

public record MenuItemCategoryUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, Guid CategoryId) : Event;
