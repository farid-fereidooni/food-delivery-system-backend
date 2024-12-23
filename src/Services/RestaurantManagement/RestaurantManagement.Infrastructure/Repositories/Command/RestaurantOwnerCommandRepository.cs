using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Models.Command.RestaurantAggregate;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public class RestaurantOwnerCommandRepository
    : BaseCommandRepository<RestaurantOwner>, IRestaurantOwnerCommandRepository
{
    private readonly CommandDbContext _dbContext;

    public RestaurantOwnerCommandRepository(CommandDbContext dbContext) : base(dbContext.RestaurantOwners)
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
