using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Read.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.DenormalizationEventHandlers;

public class MenuCategoryDenormalizationHandler :
    IEventHandler<MenuCategoryCreatedDenormalizationEvent>,
    IEventHandler<MenuCategoryUpdatedDenormalizationEvent>,
    IEventHandler<MenuCategoryRemovedDenormalizationEvent>
{
    private readonly IMenuCategoryRepository _repository;
    private readonly ILogger<MenuCategoryDenormalizationHandler> _logger;

    public MenuCategoryDenormalizationHandler(
        IMenuCategoryRepository repository, ILogger<MenuCategoryDenormalizationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(
        MenuCategoryCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.MenuCategoryId, cancellationToken))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        await _repository.AddAsync(new MenuCategory
        {
            Id = @event.MenuCategoryId,
            OwnerId = @event.OwnerId,
            Name = @event.Name,
        }, cancellationToken);
    }

    public async Task HandleAsync(
        MenuCategoryUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(@event.MenuCategoryId, cancellationToken);
        if (category == null)
        {
            _logger.LogWarning(
                "Denormalization event {eventName} Failed. Menu category with ID {id} not found",
                @event.EventName,
                @event.MenuCategoryId);
            throw new EventIgnoredException();
        }

        category.Name = @event.Name;

        await _repository.UpdateAsync(category, cancellationToken);
    }

    public async Task HandleAsync(MenuCategoryRemovedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteByIdAsync(@event.MenuCategoryId, cancellationToken);
    }
}
