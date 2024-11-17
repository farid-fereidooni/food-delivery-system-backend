using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Models.MenuAggregate;
using RestaurantManagement.Infrastructure.Database;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class MenuCommandRepository : BaseCommandRepository<Menu>, IMenuCommandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MenuCommandRepository(ApplicationDbContext dbContext) : base(dbContext.Menus)
    {
        _dbContext = dbContext;
    }

    public override async Task<Menu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Menus
            .Where(m => m.Id == id)
            .Include("MenuItems")
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Menu?> GetByIdAsync(Guid menuId, Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Menus
            .Where(m => m.Id == menuId
                && _dbContext.Restaurants.Any(r => r.Id == m.Id && r.OwnerId == ownerId))
            .Include("MenuItems")
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyMenuItemWithCategoryAsync(
        Guid menuCategoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuItems.AnyAsync(m => m.CategoryId == menuCategoryId, cancellationToken);
    }

    public async Task<bool> AnyMenuItemWithFoodAsync(Guid foodId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuItems.AnyAsync(m => m.FoodId == foodId, cancellationToken);
    }
}
