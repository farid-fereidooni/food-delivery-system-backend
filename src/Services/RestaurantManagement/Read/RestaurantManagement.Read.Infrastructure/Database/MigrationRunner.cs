using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace RestaurantManagement.Read.Infrastructure.Database;

public class MigrationRunner : IMigrationRunner
{
    private readonly IMongoDatabase _database;
    private readonly DbContext _context;
    private readonly ILogger<MigrationRunner> _logger;
    private readonly List<IMigration> _migrations;
    private const string MigrationsCollectionName = "migrations";

    public MigrationRunner(
        IMongoDatabase database,
        DbContext context,
        IEnumerable<IMigration> migrations,
        ILogger<MigrationRunner> logger)
    {
        _database = database;
        _context = context;
        _logger = logger;
        _migrations = migrations.ToList();
    }

    public async ValueTask RunMigrationsAsync(CancellationToken cancellationToken = default)
    {
        var migrationsCollection = _database.GetCollection<BsonDocument>(MigrationsCollectionName);
        var executedMigrations = await migrationsCollection
            .Find(FilterDefinition<BsonDocument>.Empty)
            .ToListAsync(cancellationToken: cancellationToken);

        var executedMigrationNames = executedMigrations
            .Select(m => m["Name"].AsString)
            .ToHashSet();

        foreach (var migration in _migrations.Where(migration => !executedMigrationNames.Contains(migration.Name)))
        {
            _logger.LogInformation("Executing migration: {migrationName}", migration.Name);
            await migration.ExecuteAsync(_context, cancellationToken);

            var migrationDocument = new BsonDocument
            {
                { "Name", migration.Name }, { "ExecutedAt", DateTime.UtcNow }
            };
            await migrationsCollection.InsertOneAsync(migrationDocument, cancellationToken: cancellationToken);
        }
    }
}
