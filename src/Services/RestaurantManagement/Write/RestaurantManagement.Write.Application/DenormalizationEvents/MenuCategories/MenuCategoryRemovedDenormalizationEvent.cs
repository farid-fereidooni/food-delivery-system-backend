using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.MenuCategories;

public record MenuCategoryRemovedDenormalizationEvent : Event
{
    public Guid MenuCategoryId { get; init; }
}
