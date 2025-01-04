using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public class RestaurantOwnerQueryRepository : BaseQueryRepository<RestaurantOwnerQuery>, IRestaurantOwnerQueryRepository
{
    private readonly QueryDbContext _dbContext;

    public RestaurantOwnerQueryRepository(QueryDbContext dbContext)
        : base(dbContext.GetCollection<RestaurantOwnerQuery>())
    {
        _dbContext = dbContext;
    }
}
