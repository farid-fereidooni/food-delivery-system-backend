namespace RestaurantManagement.Domain.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
