namespace EventBus.Core;

public interface IEventLogService
{
    Task AddEventAsync(
        IEvent @event, string topic, Guid transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> GetTransactionPendingEvents(
        Guid transactionId, CancellationToken cancellationToken = default);
    Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken);
    Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken);
    Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken);
}
