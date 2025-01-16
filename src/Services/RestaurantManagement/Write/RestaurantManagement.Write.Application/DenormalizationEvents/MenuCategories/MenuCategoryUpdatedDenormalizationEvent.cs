using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.MenuCategories;

public record MenuCategoryUpdatedDenormalizationEvent : Event
{
    public required Guid MenuCategoryId { get; init; }
    public required string Name { get; init; }
}
