using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Menus;

public record MenuItemUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, string Name, decimal Price, string? Description) : Event;
