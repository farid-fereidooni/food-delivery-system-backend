namespace RestaurantManagement.Infrastructure.Database.Query;

public interface IQueryMigration
{
    string Name { get; }
    Task ExecuteAsync(QueryDbContext database, CancellationToken cancellationToken = default);
}
