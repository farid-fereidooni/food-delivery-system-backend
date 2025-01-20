using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Write.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.FoodTypes;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Application.DomainEventHandlers.FoodTypes;

public class FoodTypeRemovedEventHandler : INotificationHandler<FoodTypeRemovedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;


    public FoodTypeRemovedEventHandler(
        IEventLogService eventLogService,
        IOptions<EventBusConfiguration> options,
        IUnitOfWork unitOfWork)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(FoodTypeRemovedEvent notification, CancellationToken cancellationToken)
    {
        var foodTypeRemovedDenormalizationEvent = new FoodTypeRemovedDenormalizationEvent
        {
            Id = notification.Id,
        };

        await _eventLogService.AddEventAsync(
            foodTypeRemovedDenormalizationEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
