namespace RestaurantManagement.Domain.Contracts;

public interface IUnitOfWork
{
    ValueTask<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Guid? CurrentTransactionId { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    ValueTask CommitTransaction(Guid transactionId, CancellationToken cancellationToken = default);
}
