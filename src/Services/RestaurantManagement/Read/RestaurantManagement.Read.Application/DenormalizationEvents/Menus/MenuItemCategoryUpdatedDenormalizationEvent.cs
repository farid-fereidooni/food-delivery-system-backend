using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Menus;

public record MenuItemCategoryUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, Guid CategoryId) : Event;
