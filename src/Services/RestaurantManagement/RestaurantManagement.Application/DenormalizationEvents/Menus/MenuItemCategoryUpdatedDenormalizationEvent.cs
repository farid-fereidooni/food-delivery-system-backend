using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Menus;

public record MenuItemCategoryUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, Guid CategoryId) : Event;
