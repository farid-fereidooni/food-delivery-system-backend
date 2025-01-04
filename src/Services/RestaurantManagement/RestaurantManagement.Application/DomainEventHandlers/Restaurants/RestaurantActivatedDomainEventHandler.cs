using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Restaurants;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.Restaurants;

public class RestaurantActivatedDomainEventHandler : INotificationHandler<RestaurantActivatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventLogService _eventLogService;
    private readonly EventBusConfiguration _eventBusConfig;

    public RestaurantActivatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IEventLogService eventLogService,
        IOptions<EventBusConfiguration> options)
    {
        _unitOfWork = unitOfWork;
        _eventLogService = eventLogService;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(RestaurantActivatedEvent notification, CancellationToken cancellationToken)
    {
        var restaurantActivatedEvent = new RestaurantActivatedDenormalizationEvent(notification.Id);

        await _eventLogService.AddEventAsync(
            restaurantActivatedEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
