using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Infrastructure.Repositories;

public class RestaurantOwnerRepository : BaseRepository<RestaurantOwner>, IRestaurantOwnerRepository
{
    private readonly DbContext _dbContext;

    public RestaurantOwnerRepository(DbContext dbContext)
        : base(dbContext.GetCollection<RestaurantOwner>())
    {
        _dbContext = dbContext;
    }
}
