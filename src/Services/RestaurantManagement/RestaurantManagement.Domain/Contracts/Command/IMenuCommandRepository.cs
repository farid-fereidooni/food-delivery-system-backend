using RestaurantManagement.Domain.Models.Command.MenuAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IMenuCommandRepository : ICommandRepository<Menu>
{
    Task<Menu?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<bool> AnyMenuItemWithCategoryAsync(Guid menuCategoryId, CancellationToken cancellationToken = default);
}
