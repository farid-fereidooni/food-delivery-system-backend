using RestaurantManagement.Core.Domain.Models.MenuAggregate;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IMenuCommandRepository : ICommandRepository<Menu>
{
    Task<Menu?> GetByIdAsync(Guid menuId, Guid ownerId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithFoodAsync(Guid foodId, CancellationToken cancellationToken = default);
}
