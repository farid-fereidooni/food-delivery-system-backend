using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Models.MenuAggregate;
using RestaurantManagement.Write.Infrastructure.Database;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Write.Infrastructure.Repositories;

public class MenuRepository : BaseRepository<Menu>, IMenuRepository
{
    private readonly DbContext _dbContext;

    public MenuRepository(DbContext dbContext) : base(dbContext.Menus)
    {
        _dbContext = dbContext;
    }

    public override async Task<Menu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Menus
            .Where(m => m.Id == id)
            .Include(i => i.MenuItems)
            .ThenInclude(i => i.FoodTypeMenuItems)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Menu?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Menus
            .Where(m => _dbContext.Restaurants.Any(r => r.Id == m.RestaurantId && r.OwnerId == ownerId))
            .Include(i => i.MenuItems)
            .ThenInclude(i => i.FoodTypeMenuItems)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyMenuItemWithCategoryAsync(
        Guid menuCategoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuItems.AnyAsync(m => m.CategoryId == menuCategoryId, cancellationToken);
    }
}
