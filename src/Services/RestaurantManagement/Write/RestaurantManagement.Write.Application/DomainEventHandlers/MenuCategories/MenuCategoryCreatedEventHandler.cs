using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.MenuCategories;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.MenuCategories;

public class MenuCategoryCreatedEventHandler : INotificationHandler<MenuCategoryCreatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuCategoryCreatedEventHandler(
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
