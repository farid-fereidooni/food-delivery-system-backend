using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.DomainEvents.Restaurants;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Command.MenuAggregate;

namespace RestaurantManagement.Application.DomainEventHandlers.Restaurants;

public class RestaurantCreatedDomainEventHandler : INotificationHandler<RestaurantCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IEventLogService _eventLogService;
    private readonly EventBusConfiguration _eventBusConfig;

    public RestaurantCreatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IEventLogService eventLogService,
        IOptions<EventBusConfiguration> options)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _eventLogService = eventLogService;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
    {
        var defaultMenu = new Menu(notification.Id);

        await _menuRepository.AddAsync(defaultMenu, cancellationToken);

        var restaurantCreatedEvent = new RestaurantCreatedDenormalizationEvent(
            notification.Id,
            notification.OwnerId,
            notification.Name,
            notification.Status,
            notification.Street,
            notification.City,
            notification.State,
            notification.ZipCode);

        await _eventLogService.AddEventAsync(
            restaurantCreatedEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
