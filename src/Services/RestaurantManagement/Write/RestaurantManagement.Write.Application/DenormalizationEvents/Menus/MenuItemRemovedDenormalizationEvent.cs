using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Menus;

public record MenuItemRemovedDenormalizationEvent(Guid Id, Guid MenuId, Guid RestaurantId) : Event;
