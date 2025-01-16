using RestaurantManagement.Write.Domain.Models.MenuAggregate;

namespace RestaurantManagement.Write.Domain.Contracts.Repositories;

public interface IMenuRepository : IRepository<Menu>
{
    Task<Menu?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default);
}
