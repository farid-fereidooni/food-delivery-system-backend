using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Infrastructure.Database;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class MenuCategoryCommandRepository : BaseCommandRepository<MenuCategory>, IMenuCategoryCommandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MenuCategoryCommandRepository(ApplicationDbContext dbContext) : base(dbContext.MenuCategories)
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
