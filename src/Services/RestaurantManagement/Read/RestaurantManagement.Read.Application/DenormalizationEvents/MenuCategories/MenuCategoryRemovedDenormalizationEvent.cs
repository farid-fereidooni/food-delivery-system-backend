using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.MenuCategories;

public record MenuCategoryRemovedDenormalizationEvent : Event
{
    public Guid MenuCategoryId { get; init; }
}
