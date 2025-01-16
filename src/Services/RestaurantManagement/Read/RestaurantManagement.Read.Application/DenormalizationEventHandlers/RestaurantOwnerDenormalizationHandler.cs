using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.DenormalizationEventHandlers;

public class RestaurantOwnerDenormalizationHandler : IEventHandler<RestaurantOwnerCreatedDenormalizationEvent>
{
    private readonly IRestaurantOwnerRepository _repository;
    private readonly ILogger<RestaurantOwnerDenormalizationHandler> _logger;

    public RestaurantOwnerDenormalizationHandler(
        IRestaurantOwnerRepository repository,
        ILogger<RestaurantOwnerDenormalizationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(
        RestaurantOwnerCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.Id, cancellationToken))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        await _repository.AddAsync(new RestaurantOwner(@event.Id), cancellationToken);
    }
}
