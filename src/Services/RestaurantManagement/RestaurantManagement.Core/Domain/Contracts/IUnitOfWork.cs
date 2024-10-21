namespace RestaurantManagement.Core.Domain.Contracts;

public interface IUnitOfWork
{
    void Commit();
}
