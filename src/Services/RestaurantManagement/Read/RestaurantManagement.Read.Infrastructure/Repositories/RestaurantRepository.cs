using MongoDB.Driver;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Infrastructure.Repositories;

public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
{
    private readonly DbContext _dbContext;

    public RestaurantRepository(DbContext dbContext) : base(dbContext.GetCollection<Restaurant>())
    {
        _dbContext = dbContext;
    }

    public async Task<Restaurant?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.GetCollection<Restaurant>()
            .Find(Builders<Restaurant>.Filter.Eq(x => x.OwnerId, ownerId))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<RestaurantMenuItem>> GetMenuItemsByOwnerIdAsync(
        Guid ownerId, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.GetCollection<Restaurant>()
            .Find(Builders<Restaurant>.Filter.Eq(x => x.OwnerId, ownerId))
            .Project(s => s.Menus.FirstOrDefault())
            .SingleOrDefaultAsync(cancellationToken);

        return menu == null ? [] : menu.MenuItems;
    }

    public async Task<ICollection<Restaurant>> GetByMenuCategoryIdAsync(
        Guid menuCategoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<Restaurant>()
            .Find(Builders<Restaurant>.Filter.ElemMatch(
                $"{nameof(Restaurant.Menus)}.{nameof(RestaurantMenu.MenuItems)}",
                Builders<RestaurantMenuItem>.Filter.Eq(x => x.CategoryId, menuCategoryId)))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<Restaurant>> GetByFoodTypeIdAsync(
        Guid foodTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<Restaurant>()
            .Find(Builders<Restaurant>.Filter.ElemMatch(
                $"{nameof(Restaurant.Menus)}.{nameof(RestaurantMenu.MenuItems)}.{nameof(RestaurantMenuItem.FoodTypes)}",
                Builders<RestaurantFoodTypes>.Filter.Eq(x => x.Id, foodTypeId)))
            .ToListAsync(cancellationToken);
    }
}
