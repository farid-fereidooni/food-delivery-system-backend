using MongoDB.Driver;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Infrastructure.QueryMigrations;

public class AddRequiredIndexesToRestaurant : IMigration
{
    public string Name => $"1_{nameof(AddRequiredIndexesToRestaurant)}";
    public async Task ExecuteAsync(DbContext database, CancellationToken cancellationToken = default)
    {
        await CreateAscendingIndex(
            $"{nameof(Restaurant.Menus)}.{nameof(RestaurantMenu.MenuItems)}.{nameof(RestaurantMenuItem.CategoryId)}",
            database,
            cancellationToken);

        await CreateAscendingIndex(
            string.Format("{0}.{1}.{2}.{3}",
                nameof(Restaurant.Menus),
                nameof(RestaurantMenu.MenuItems),
                nameof(RestaurantMenuItem.FoodTypes),
                nameof(RestaurantFoodTypes.Id)),
            database,
            cancellationToken);
    }

    private static async ValueTask CreateAscendingIndex(string index,
        DbContext database,
        CancellationToken token = default)
    {
        var collection = database.GetCollection<Restaurant>();
        var indexKeys = Builders<Restaurant>.IndexKeys.Ascending(index);
        var indexModel = new CreateIndexModel<Restaurant>(indexKeys);

        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: token);
    }
}
