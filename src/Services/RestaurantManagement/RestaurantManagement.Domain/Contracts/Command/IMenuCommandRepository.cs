using RestaurantManagement.Domain.Models.Command.MenuAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IMenuCommandRepository : ICommandRepository<Menu>
{
    Task<Menu?> GetByIdAsync(Guid menuId, Guid ownerId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithFoodAsync(Guid foodId, CancellationToken cancellationToken = default);
}
