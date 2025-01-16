using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.Menus;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.Menus;

public class MenuItemStockUpdatedEventHandler : INotificationHandler<MenuItemStockUpdatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuItemStockUpdatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuItemStockUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new MenuItemStockUpdatedDenormalizationEvent(
            notification.Id, notification.MenuId, notification.RestaurantId, notification.Stock);

        await _eventLogService.AddEventAsync(
            @event,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
