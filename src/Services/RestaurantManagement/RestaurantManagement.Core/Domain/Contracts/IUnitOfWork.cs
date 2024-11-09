namespace RestaurantManagement.Core.Domain.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
