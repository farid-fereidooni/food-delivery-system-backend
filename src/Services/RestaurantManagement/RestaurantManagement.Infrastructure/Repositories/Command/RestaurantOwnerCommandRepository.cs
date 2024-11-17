using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;
using RestaurantManagement.Infrastructure.Database;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class RestaurantOwnerCommandRepository
    : BaseCommandRepository<RestaurantOwner>, IRestaurantOwnerCommandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RestaurantOwnerCommandRepository(ApplicationDbContext dbContext) : base(dbContext.RestaurantOwners)
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
