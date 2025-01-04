using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Menus;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.Menus;

public class MenuItemUpdatedEventHandler : INotificationHandler<MenuItemUpdatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuItemUpdatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuItemUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new MenuItemUpdatedDenormalizationEvent(
            notification.Id,
            notification.MenuId,
            notification.RestaurantId,
            notification.Name,
            notification.Price,
            notification.Description);

        await _eventLogService.AddEventAsync(
            @event,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
