using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Models.Command.MenuAggregate;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class MenuCommandRepository : BaseCommandRepository<Menu>, IMenuCommandRepository
{
    private readonly CommandDbContext _dbContext;

    public MenuCommandRepository(CommandDbContext dbContext) : base(dbContext.Menus)
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
