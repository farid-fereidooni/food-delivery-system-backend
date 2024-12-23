using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Domain.Contracts.Query;

public interface IMenuCategoryQueryRepository : IQueryRepository<MenuCategoryQuery>
{
    Task<ICollection<MenuCategoryQuery>> GetByOwnerId(Guid ownerId, CancellationToken cancellationToken);
}
