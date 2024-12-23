using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.MenuCategories;

public record MenuCategoryCreatedDenormalizationEvent : Event
{
    public required Guid MenuCategoryId { get; init; }

    public required Guid OwnerId { get; init; }
    public required string Name { get; init; }
}
