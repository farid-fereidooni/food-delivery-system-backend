using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public class MenuCategoryQueryRepository : BaseQueryRepository<MenuCategoryQuery>, IMenuCategoryQueryRepository
{
    private readonly QueryDbContext _dbContext;

    public MenuCategoryQueryRepository(QueryDbContext dbContext) : base(dbContext.GetCollection<MenuCategoryQuery>())
    {
        _dbContext = dbContext;
    }


    public async Task<ICollection<MenuCategoryQuery>> GetByOwnerId(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<MenuCategoryQuery>()
            .Find(f => f.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
