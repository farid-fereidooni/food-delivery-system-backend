namespace RestaurantManagement.Read.Infrastructure.Database;

public interface IMigration
{
    string Name { get; }
    Task ExecuteAsync(DbContext database, CancellationToken cancellationToken = default);
}
