using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Models.RestaurantAggregate;
using RestaurantManagement.Write.Infrastructure.Database;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Write.Infrastructure.Repositories;

public class RestaurantOwnerRepository
    : BaseRepository<RestaurantOwner>, IRestaurantOwnerRepository
{
    private readonly DbContext _dbContext;

    public RestaurantOwnerRepository(DbContext dbContext) : base(dbContext.RestaurantOwners)
    {
        _dbContext = dbContext;
    }

    public override async Task<RestaurantOwner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RestaurantOwners
            .Where(r => r.Id == id)
            .Include("Restaurants")
            .SingleOrDefaultAsync(cancellationToken);
    }
}
