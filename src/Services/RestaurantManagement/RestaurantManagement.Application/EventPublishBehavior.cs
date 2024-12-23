using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Application;

public class EventPublishBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventLogService _eventLogService;
    private readonly IEventBus _eventBus;
    private readonly ILogger<EventPublishBehavior<TRequest, TResponse>> _logger;

    public EventPublishBehavior(
        IUnitOfWork unitOfWork,
        IEventLogService eventLogService,
        IEventBus eventBus,
        ILogger<EventPublishBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _eventLogService = eventLogService;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var transactionId = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var response = await next();
        await _unitOfWork.CommitTransaction(transactionId, cancellationToken);

        await PublishEvents(transactionId, cancellationToken);

        return response;
    }

    private async Task PublishEvents(Guid transactionId, CancellationToken cancellationToken)
    {
        var events = await _eventLogService.GetTransactionPendingEvents(transactionId, cancellationToken);
        foreach (var @event in events)
        {
            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(@event.EventId, cancellationToken);
                await _eventBus.PublishAsync(@event, cancellationToken);
                await _eventLogService.MarkEventAsPublishedAsync(@event.EventId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Publishing {eventName} event failed", @event.EventName);
                await _eventLogService.MarkEventAsFailedAsync(@event.EventId, cancellationToken);
            }
        }
    }
}
