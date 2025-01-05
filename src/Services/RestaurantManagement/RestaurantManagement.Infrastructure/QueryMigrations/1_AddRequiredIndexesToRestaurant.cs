using MongoDB.Driver;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Infrastructure.QueryMigrations;

public class AddRequiredIndexesToRestaurant : IQueryMigration
{
    public string Name => $"1_{nameof(AddRequiredIndexesToRestaurant)}";
    public async Task ExecuteAsync(QueryDbContext database, CancellationToken cancellationToken = default)
    {
        await CreateAscendingIndex(
            $"{nameof(RestaurantQuery.Menus)}.{nameof(RestaurantMenuQuery.MenuItems)}.{nameof(RestaurantMenuItemQuery.CategoryId)}",
            database,
            cancellationToken);

        await CreateAscendingIndex(
            string.Format("{0}.{1}.{2}.{3}",
                nameof(RestaurantQuery.Menus),
                nameof(RestaurantMenuQuery.MenuItems),
                nameof(RestaurantMenuItemQuery.FoodTypes),
                nameof(RestaurantFoodTypesQuery.Id)),
            database,
            cancellationToken);
    }

    private static async ValueTask CreateAscendingIndex(string index,
        QueryDbContext database,
        CancellationToken token = default)
    {
        var collection = database.GetCollection<RestaurantQuery>();
        var indexKeys = Builders<RestaurantQuery>.IndexKeys.Ascending(index);
        var indexModel = new CreateIndexModel<RestaurantQuery>(indexKeys);

        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: token);
    }
}
