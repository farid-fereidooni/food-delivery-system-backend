using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public class FoodTypeQueryRepository : BaseQueryRepository<FoodTypeQuery>, IFoodTypeQueryRepository
{
    private readonly QueryDbContext _dbContext;

    public FoodTypeQueryRepository(QueryDbContext dbContext) : base(dbContext.GetCollection<FoodTypeQuery>())
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<FoodTypeQuery>> GetByIdsAsync(
        ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<FoodTypeQuery>()
            .Find(Builders<FoodTypeQuery>.Filter.In(x => x.Id, ids))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<FoodTypeQuery>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<FoodTypeQuery>()
            .Find(x => true)
            .ToListAsync(cancellationToken);
    }
}
