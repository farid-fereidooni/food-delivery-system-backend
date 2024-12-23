using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Models.Command.FoodAggregate;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class FoodCommandRepository : BaseCommandRepository<Food>, IFoodCommandRepository
{
    private readonly CommandDbContext _dbContext;

    public FoodCommandRepository(CommandDbContext dbContext) : base(dbContext.Foods)
    {
        _dbContext = dbContext;
    }

    public override async Task<Food?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Foods
            .Where(f => f.Id == id)
            .Include("FoodTypeFoods")
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsWithName(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Foods.Where(f => f.OwnerId == ownerId && f.Specification.Name == name);
        if (excludeId.HasValue)
            query = query.Where(f => f.Id != excludeId);

        return await query.AnyAsync(cancellationToken);
    }
}
