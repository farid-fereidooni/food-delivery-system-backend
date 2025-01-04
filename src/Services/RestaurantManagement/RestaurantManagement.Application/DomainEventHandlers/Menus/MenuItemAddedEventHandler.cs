using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Menus;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.Menus;

public class MenuItemAddedEventHandler : INotificationHandler<MenuItemAddedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuItemAddedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuItemAddedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new MenuItemAddedDenormalizationEvent(
            notification.Id,
            notification.MenuId,
            notification.RestaurantId,
            notification.CategoryId,
            notification.Name,
            notification.Price,
            notification.Description,
            notification.Stock,
            notification.FoodTypeIds);

        await _eventLogService.AddEventAsync(
            @event,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
