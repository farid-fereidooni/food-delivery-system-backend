using RestaurantManagement.Core.Domain.Models.MenuCategoryAggregate;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IMenuCategoryCommandRepository : ICommandRepository<MenuCategory>
{
    Task<bool> ExistsWithNameAsync(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default);
}
