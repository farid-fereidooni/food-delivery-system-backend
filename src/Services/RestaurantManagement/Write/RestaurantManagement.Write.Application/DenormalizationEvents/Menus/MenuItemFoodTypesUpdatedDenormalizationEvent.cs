using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Menus;

public record MenuItemFoodTypesUpdatedDenormalizationEvent(
    Guid Id, Guid MenuId, Guid RestaurantId, ICollection<Guid> FoodTypes) : Event;
