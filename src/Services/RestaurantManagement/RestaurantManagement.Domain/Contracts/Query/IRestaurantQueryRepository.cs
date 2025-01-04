using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Domain.Contracts.Query;

public interface IRestaurantQueryRepository : IQueryRepository<RestaurantQuery>
{
    Task<RestaurantQuery> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);

    Task<ICollection<RestaurantMenuItemQuery>> GetMenuItemsByOwnerIdAsync(
        Guid ownerId, CancellationToken cancellationToken);

    Task<ICollection<RestaurantQuery>> GetByMenuCategoryIdAsync(
        Guid menuCategoryId, CancellationToken cancellationToken);

    Task<ICollection<RestaurantQuery>> GetByFoodTypeIdAsync(Guid foodTypeId, CancellationToken cancellationToken);
}
