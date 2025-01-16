using MongoDB.Driver;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Infrastructure.Repositories;

public class MenuCategoryRepository : BaseRepository<MenuCategory>, IMenuCategoryRepository
{
    private readonly DbContext _dbContext;

    public MenuCategoryRepository(DbContext dbContext) : base(dbContext.GetCollection<MenuCategory>())
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<MenuCategory>> GetByOwnerId(
        Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<MenuCategory>()
            .Find(f => f.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
