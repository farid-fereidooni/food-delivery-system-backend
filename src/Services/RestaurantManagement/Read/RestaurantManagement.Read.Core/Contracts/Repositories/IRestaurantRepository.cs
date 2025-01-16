using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Domain.Contracts.Repositories;

public interface IRestaurantRepository : IRepository<Restaurant>
{
    Task<Restaurant?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);

    Task<ICollection<RestaurantMenuItem>> GetMenuItemsByOwnerIdAsync(
        Guid ownerId, CancellationToken cancellationToken);

    Task<ICollection<Restaurant>> GetByMenuCategoryIdAsync(
        Guid menuCategoryId, CancellationToken cancellationToken);

    Task<ICollection<Restaurant>> GetByFoodTypeIdAsync(Guid foodTypeId, CancellationToken cancellationToken);
}
