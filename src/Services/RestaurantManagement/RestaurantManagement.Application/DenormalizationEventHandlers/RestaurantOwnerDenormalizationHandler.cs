using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.DenormalizationEventHandlers;

public class RestaurantOwnerDenormalizationHandler : IEventHandler<RestaurantOwnerCreatedDenormalizationEvent>
{
    private readonly IRestaurantOwnerQueryRepository _repository;
    private readonly ILogger<RestaurantOwnerDenormalizationHandler> _logger;

    public RestaurantOwnerDenormalizationHandler(
        IRestaurantOwnerQueryRepository repository,
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

        await _repository.AddAsync(new RestaurantOwnerQuery(@event.Id), cancellationToken);
    }
}
