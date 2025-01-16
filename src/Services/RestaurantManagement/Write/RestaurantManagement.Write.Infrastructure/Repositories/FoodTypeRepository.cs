using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Write.Infrastructure.Database;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Write.Infrastructure.Repositories;

public class FoodTypeRepository : BaseRepository<FoodType>, IFoodTypeRepository
{
    private readonly DbContext _dbContext;

    public FoodTypeRepository(DbContext dbContext) : base(dbContext.FoodTypes)
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
