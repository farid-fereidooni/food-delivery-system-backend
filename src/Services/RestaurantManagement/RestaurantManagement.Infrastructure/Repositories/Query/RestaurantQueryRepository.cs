using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public class RestaurantQueryRepository : BaseQueryRepository<RestaurantQuery>, IRestaurantQueryRepository
{
    private readonly QueryDbContext _dbContext;

    public RestaurantQueryRepository(QueryDbContext dbContext) : base(dbContext.GetCollection<RestaurantQuery>())
    {
        _dbContext = dbContext;
    }

    public async Task<RestaurantQuery?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<RestaurantQuery>()
            .Find(Builders<RestaurantQuery>.Filter.Eq(x => x.OwnerId, ownerId))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<RestaurantMenuItemQuery>> GetMenuItemsByOwnerIdAsync(
        Guid ownerId, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.GetCollection<RestaurantQuery>()
            .Find(Builders<RestaurantQuery>.Filter.Eq(x => x.OwnerId, ownerId))
            .Project(s => s.Menus.FirstOrDefault())
            .SingleOrDefaultAsync(cancellationToken);

        return menu == null ? [] : menu.MenuItems;
    }

    public async Task<ICollection<RestaurantQuery>> GetByMenuCategoryIdAsync(
        Guid menuCategoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<RestaurantQuery>()
            .Find(Builders<RestaurantQuery>.Filter.ElemMatch(
                $"{nameof(RestaurantQuery.Menus)}.{nameof(RestaurantMenuQuery.MenuItems)}",
                Builders<RestaurantMenuItemQuery>.Filter.Eq(x => x.CategoryId, menuCategoryId)))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<RestaurantQuery>> GetByFoodTypeIdAsync(
        Guid foodTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<RestaurantQuery>()
            .Find(Builders<RestaurantQuery>.Filter.ElemMatch(
                $"{nameof(RestaurantQuery.Menus)}.{nameof(RestaurantMenuQuery.MenuItems)}.{nameof(RestaurantMenuItemQuery.FoodTypes)}",
                Builders<RestaurantFoodTypesQuery>.Filter.Eq(x => x.Id, foodTypeId)))
            .ToListAsync(cancellationToken);
    }
}
