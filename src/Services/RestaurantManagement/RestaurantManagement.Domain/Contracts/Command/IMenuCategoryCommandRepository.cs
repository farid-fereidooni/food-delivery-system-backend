using RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IMenuCategoryCommandRepository : ICommandRepository<MenuCategory>
{
    Task<bool> ExistsWithNameAsync(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default);
}
