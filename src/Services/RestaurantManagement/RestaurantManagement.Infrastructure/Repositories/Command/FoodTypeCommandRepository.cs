using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class FoodTypeCommandRepository : BaseCommandRepository<FoodType>, IFoodTypeCommandRepository
{
    private readonly CommandDbContext _dbContext;

    public FoodTypeCommandRepository(CommandDbContext dbContext) : base(dbContext.FoodTypes)
    {
        _dbContext = dbContext;
    }

    public override async Task<FoodType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FoodTypes
            .SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FoodTypes.Where(f => id.Contains(f.Id)).CountAsync(cancellationToken) == id.Count();
    }
}
