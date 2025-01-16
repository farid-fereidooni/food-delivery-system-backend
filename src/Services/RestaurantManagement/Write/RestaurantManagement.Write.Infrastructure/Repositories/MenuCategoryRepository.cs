using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Write.Infrastructure.Database;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Write.Infrastructure.Repositories;

public class MenuCategoryRepository : BaseRepository<MenuCategory>, IMenuCategoryRepository
{
    private readonly DbContext _dbContext;

    public MenuCategoryRepository(DbContext dbContext) : base(dbContext.MenuCategories)
    {
        _dbContext = dbContext;
    }

    public override async Task<MenuCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuCategories.FindAsync([id], cancellationToken);
    }

    public async Task<bool> ExistsWithNameAsync(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.MenuCategories.Where(m => m.Name == name && m.OwnerId == ownerId);
        if (excludeId.HasValue)
            query = query.Where(m => m.Id != excludeId);

        return await query.AnyAsync(cancellationToken);
    }
}
