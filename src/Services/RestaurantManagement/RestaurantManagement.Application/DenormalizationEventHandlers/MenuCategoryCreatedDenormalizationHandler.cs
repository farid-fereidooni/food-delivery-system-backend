using EventBus.Core;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.DenormalizationEventHandlers;

public class MenuCategoryCreatedDenormalizationHandler : IEventHandler<MenuCategoryCreatedDenormalizationEvent>
{
    private readonly IMenuCategoryQueryRepository _repository;

    public MenuCategoryCreatedDenormalizationHandler(IMenuCategoryQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        MenuCategoryCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.MenuCategoryId, cancellationToken))
            return;

        await _repository.AddAsync(new MenuCategoryQuery
        {
            Id = @event.MenuCategoryId,
            OwnerId = @event.OwnerId,
            Name = @event.Name,
        }, cancellationToken);
    }
}
