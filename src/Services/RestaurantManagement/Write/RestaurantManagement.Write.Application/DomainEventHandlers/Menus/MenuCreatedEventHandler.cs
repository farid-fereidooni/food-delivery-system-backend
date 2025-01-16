using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.Menus;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.Menus;

public class MenuCreatedEventHandler : INotificationHandler<MenuCreatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuCreatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuCreatedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new MenuCreatedDenormalizationEvent(notification.Id, notification.RestaurantId);

        await _eventLogService.AddEventAsync(
            @event,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
