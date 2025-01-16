namespace RestaurantManagement.Read.Infrastructure.Database;

public interface IMigrationRunner
{
    ValueTask RunMigrationsAsync(CancellationToken cancellationToken);
}
