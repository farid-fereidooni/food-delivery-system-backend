using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Domain.Contracts.Repositories;

public interface IMenuCategoryRepository : IRepository<MenuCategory>
{
    Task<ICollection<MenuCategory>> GetByOwnerId(Guid ownerId, CancellationToken cancellationToken);
}
