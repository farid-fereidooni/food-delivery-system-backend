using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Menus;

public record MenuItemStockUpdatedDenormalizationEvent(Guid Id, Guid MenuId, Guid RestaurantId, uint Stock) : Event;
