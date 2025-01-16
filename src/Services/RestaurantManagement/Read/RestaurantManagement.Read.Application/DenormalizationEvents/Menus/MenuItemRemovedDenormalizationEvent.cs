using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Menus;

public record MenuItemRemovedDenormalizationEvent(Guid Id, Guid MenuId, Guid RestaurantId) : Event;
