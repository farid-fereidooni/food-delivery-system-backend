using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.MenuCategories;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.MenuCategories;

public class MenuCategoryCreatedEventsHandler : INotificationHandler<MenuCategoryCreatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuCategoryCreatedEventsHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuCategoryCreatedEvent notification, CancellationToken cancellationToken)
    {
        var menuCreatedDenormalizationEvent = new MenuCategoryCreatedDenormalizationEvent
        {
            Name = notification.Name,
            OwnerId = notification.OwnerId,
            MenuCategoryId = notification.Id
        };

        await _eventLogService.AddEventAsync(
            menuCreatedDenormalizationEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
