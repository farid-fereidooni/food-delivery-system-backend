using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.FoodTypes;
using RestaurantManagement.Domain.DomainEvents.MenuCategories;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.FoodTypes;

public class FoodTypeUpdatedEventHandler : INotificationHandler<FoodTypeUpdatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public FoodTypeUpdatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(FoodTypeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var foodTypeUpdatedDenormalizationEvent = new FoodTypeUpdatedDenormalizationEvent
        {
            Id = notification.Id,
            Name = notification.Name,
        };

        await _eventLogService.AddEventAsync(
            foodTypeUpdatedDenormalizationEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
