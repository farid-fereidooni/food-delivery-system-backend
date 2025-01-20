using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Read.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.DenormalizationEventHandlers;

public class FoodTypeDenormalizationHandler:
    IEventHandler<FoodTypeCreatedDenormalizationEvent>,
    IEventHandler<FoodTypeUpdatedDenormalizationEvent>,
    IEventHandler<FoodTypeRemovedDenormalizationEvent>
{
    private readonly IFoodTypeRepository _repository;
    private readonly ILogger<FoodTypeDenormalizationHandler> _logger;

    public FoodTypeDenormalizationHandler(
        IFoodTypeRepository repository, ILogger<FoodTypeDenormalizationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(FoodTypeCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.Id, cancellationToken))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        await _repository.AddAsync(new FoodType
        {
            Id = @event.Id,
            Name = @event.Name
        }, cancellationToken);
    }

    public async Task HandleAsync(FoodTypeUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var foodType = await _repository.GetByIdAsync(@event.Id, cancellationToken);
        if (foodType == null)
        {
            _logger.LogWarning(
                "Denormalization event {eventName} Failed. Food type with ID {id} not found",
                @event.EventName,
                @event.Id);
            throw new EventIgnoredException();
        }

        foodType.Name = @event.Name;
        await _repository.UpdateAsync(foodType, cancellationToken);
    }

    public async Task HandleAsync(FoodTypeRemovedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteByIdAsync(@event.Id, cancellationToken);
    }
}
