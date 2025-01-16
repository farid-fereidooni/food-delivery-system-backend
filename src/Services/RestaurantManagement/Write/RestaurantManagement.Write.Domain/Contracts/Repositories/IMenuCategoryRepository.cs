using RestaurantManagement.Write.Domain.Models.MenuCategoryAggregate;

namespace RestaurantManagement.Write.Domain.Contracts.Repositories;

public interface IMenuCategoryRepository : IRepository<MenuCategory>
{
    Task<bool> ExistsWithNameAsync(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default);
}
