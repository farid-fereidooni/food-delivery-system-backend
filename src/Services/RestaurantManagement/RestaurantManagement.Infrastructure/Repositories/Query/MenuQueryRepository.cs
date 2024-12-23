using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public class MenuQueryRepository : BaseQueryRepository<MenuQuery>, IMenuQueryRepository
{
    private readonly QueryDbContext _dbContext;

    public MenuQueryRepository(QueryDbContext dbContext) : base(dbContext.GetCollection<MenuQuery>())
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<MenuQuery>> GetByRestaurantIdAsync(Guid restaurantId)
    {
        return await _dbContext.GetCollection<MenuQuery>()
            .Find(Builders<MenuQuery>.Filter.Eq(x => x.RestaurantId, restaurantId)).ToListAsync();
    }
}
