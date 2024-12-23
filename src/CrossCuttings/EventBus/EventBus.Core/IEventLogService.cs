namespace EventBus.Core;

public interface IEventLogService
{
    Task AddEventAsync(
        Event @event, string topic, Guid transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> GetTransactionPendingEvents(
        Guid transactionId, CancellationToken cancellationToken = default);
    Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default);
}
