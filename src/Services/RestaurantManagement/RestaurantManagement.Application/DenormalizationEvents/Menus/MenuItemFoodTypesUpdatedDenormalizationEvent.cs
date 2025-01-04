using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Menus;

public record MenuItemFoodTypesUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, ICollection<Guid> FoodTypes) : Event;
