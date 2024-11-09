namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface ICommandRepository<T> where T : AggregateRoot
{
   Task<T?> GetByIdAsync(Guid id);
   Task AddAsync(T entity);
   Task DeleteByIdAsync(Guid id);
   Task DeleteAsync(T entity);
}
