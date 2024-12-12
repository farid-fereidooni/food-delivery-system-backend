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
        await _unitOfWork.SaveAsync(cancellationToken);
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

    public async Task<IEnumerable<Message>> GetFailedEvents(CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.EventLogs
            .OrderBy(x => x.EventDate)
            .Where(e => e.State == EventStateEnum.PublishedFailed)
            .ToListAsync(cancellationToken);

        return logs.Select(s => new Message
        {
            EventName = s.EventName,
            Content = s.Content,
            Topic = s.Topic,
        });
    }

    public Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return UpdateEventStatus(eventId, EventStateEnum.Published, cancellationToken);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return UpdateEventStatus(eventId, EventStateEnum.InProgress, cancellationToken);
    }

    public Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed, cancellationToken);
    }

    private async Task UpdateEventStatus(Guid eventId, EventStateEnum status, CancellationToken cancellationToken)
    {
        var eventLogEntry = _unitOfWork.EventLogs.Single(ie => ie.EventId == eventId);
        eventLogEntry.State = status;

        if (status == EventStateEnum.InProgress)
            eventLogEntry.TimesSent++;

        _unitOfWork.EventLogs.Update(eventLogEntry);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
