using MongoDB.Driver;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Infrastructure.Repositories;

public class FoodTypeRepository : BaseRepository<FoodType>, IFoodTypeRepository
{
    private readonly DbContext _dbContext;

    public FoodTypeRepository(DbContext dbContext) : base(dbContext.GetCollection<FoodType>())
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<FoodType>> GetByIdsAsync(
        ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<FoodType>()
            .Find(Builders<FoodType>.Filter.In(x => x.Id, ids))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<FoodType>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<FoodType>()
            .Find(x => true)
            .ToListAsync(cancellationToken);
    }
}
