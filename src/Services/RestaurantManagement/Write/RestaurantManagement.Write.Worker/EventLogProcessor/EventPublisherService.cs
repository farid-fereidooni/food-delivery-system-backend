using EventBus.Core;

namespace RestaurantManagement.Write.Worker.EventLogProcessor;

public interface IEventPublisherService
{
    Task PublishEvents(CancellationToken cancellationToken);
}

public class EventPublisherService : IEventPublisherService
{
    private readonly IEventLogService _eventLogService;
    private readonly IEventBus _eventBus;
    private readonly ILogger<EventPublisherService> _logger;
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public EventPublisherService(
        IEventLogService eventLogService,
        IEventBus eventBus,
        ILogger<EventPublisherService> logger)
    {
        _eventLogService = eventLogService;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task PublishEvents(CancellationToken cancellationToken)
    {
        if (!Semaphore.Wait(0, cancellationToken)) return;

        try
        {
            var events = await _eventLogService.GetPendingEvents(cancellationToken);
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
        finally
        {
            Semaphore.Release();
        }
    }
}
