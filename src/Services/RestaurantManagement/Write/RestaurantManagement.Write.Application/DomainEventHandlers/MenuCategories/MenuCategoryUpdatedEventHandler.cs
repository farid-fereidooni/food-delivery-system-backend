using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.MenuCategories;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.MenuCategories;

public class MenuCategoryUpdatedEventHandler : INotificationHandler<MenuCategoryUpdatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public MenuCategoryUpdatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(MenuCategoryUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var menuUpdatedDenormalizationEvent = new MenuCategoryUpdatedDenormalizationEvent
        {
            Name = notification.Name,
            MenuCategoryId = notification.Id
        };

        await _eventLogService.AddEventAsync(
            menuUpdatedDenormalizationEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
