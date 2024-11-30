namespace RestaurantManagement.Domain.Contracts.Command;

public interface ICommandRepository<T> where T : AggregateRoot
{
   Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
   Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
   Task AddAsync(T entity, CancellationToken cancellationToken = default);
   Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
   Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
