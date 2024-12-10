using EventBus.Core;
using EventBus.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Logging;

public class EventLogService : IEventLogService
{
    private readonly IEventLogUnitOfWork _unitOfWork;

    public EventLogService(IEventLogUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddEventAsync(
        IEvent @event, string topic, Guid transactionId, CancellationToken cancellationToken = default)
    {
        var eventLog = new EventLog(@event, topic, transactionId);
        await _unitOfWork.EventLogs.AddAsync(eventLog, cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetTransactionPendingEvents(
        Guid transactionId, CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.EventLogs
            .OrderBy(x => x.EventDate)
            .Where(e => e.TransactionId == transactionId && e.State == EventStateEnum.NotPublished)
            .ToListAsync(cancellationToken);

        return logs.Select(s => new Message
        {
            EventName = s.EventName,
            Content = s.Content,
            Topic = s.Topic,
        });
    }

    public void MarkEventAsPublishedAsync(Guid eventId)
    {
        UpdateEventStatus(eventId, EventStateEnum.Published);
    }

    public void MarkEventAsInProgressAsync(Guid eventId)
    {
        UpdateEventStatus(eventId, EventStateEnum.InProgress);
    }

    public void MarkEventAsFailedAsync(Guid eventId)
    {
        UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
    }

    private void UpdateEventStatus(Guid eventId, EventStateEnum status)
    {
        var eventLogEntry = _unitOfWork.EventLogs.Single(ie => ie.EventId == eventId);
        eventLogEntry.State = status;

        if (status == EventStateEnum.InProgress)
            eventLogEntry.TimesSent++;

        _unitOfWork.EventLogs.Update(eventLogEntry);
    }
}
