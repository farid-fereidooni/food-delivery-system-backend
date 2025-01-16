using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.DomainEvents.Restaurants;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Models.MenuAggregate;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.Restaurants;

public class RestaurantCreatedDomainEventHandler : INotificationHandler<RestaurantCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuRepository _menuRepository;
    private readonly IEventLogService _eventLogService;
    private readonly EventBusConfiguration _eventBusConfig;

    public RestaurantCreatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IMenuRepository menuRepository,
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
