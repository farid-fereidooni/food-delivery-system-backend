using MongoDB.Driver;

namespace RestaurantManagement.Infrastructure.Database.Query;

public interface IMigrationRunner
{
    ValueTask RunMigrationsAsync(CancellationToken cancellationToken);
}
