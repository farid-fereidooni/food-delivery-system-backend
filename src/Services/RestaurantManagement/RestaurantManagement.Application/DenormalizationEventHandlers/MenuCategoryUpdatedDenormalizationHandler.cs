using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Contracts.Query;

namespace RestaurantManagement.Application.DenormalizationEventHandlers;

public class MenuCategoryUpdatedDenormalizationHandler : IEventHandler<MenuCategoryUpdatedDenormalizationEvent>
{
    private readonly IMenuCategoryQueryRepository _repository;
    private readonly ILogger<MenuCategoryUpdatedDenormalizationHandler> _logger;

    public MenuCategoryUpdatedDenormalizationHandler(
        IMenuCategoryQueryRepository repository, ILogger<MenuCategoryUpdatedDenormalizationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(
        MenuCategoryUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(@event.MenuCategoryId, cancellationToken);
        if (category == null)
        {
            _logger.LogWarning("Category with id {Id} not found", @event.MenuCategoryId);
            throw new EventIgnoredException();
        }

        category.Name = @event.Name;

        await _repository.UpdateAsync(category, cancellationToken);
    }
}
