using EventBus.Models;
using Npgsql;
using Npgsql.Replication;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;
using Polly;
using RestaurantManagement.Write.Infrastructure.Database;

namespace RestaurantManagement.Write.Worker.EventLogProcessor;

public class ReplicationEventLogProcessor : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;
    private readonly ILogger<ReplicationEventLogProcessor> _logger;
    private readonly string _connectionString;
    private const string PublicationName = "event_log_pub";
    private const string PublicationSlotName = "event_log_slot";

    public ReplicationEventLogProcessor(
        IServiceProvider services,
        IConfiguration config,
        ILogger<ReplicationEventLogProcessor> logger)
    {
        _services = services;
        _logger = logger;
        _connectionString = config.GetConnectionString("Database") ?? throw new NullReferenceException();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting replication event publisher service");
        await using (var scope = _services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            var eventLogTableName = dbContext.GetTableName<EventLog>();

            await HandleSqlException(_logger)
                .ExecuteAsync(_ => Initialize(eventLogTableName, stoppingToken), stoppingToken);
        }

        await HandleSqlException(_logger)
            .ExecuteAsync(_ => StartReplication(stoppingToken), stoppingToken);
    }

    private async Task StartReplication(CancellationToken cancellationToken)
    {
        await using var connection = new LogicalReplicationConnection(_connectionString);
        await connection.Open(cancellationToken);

        var slot = new PgOutputReplicationSlot(PublicationSlotName);
        var options = new PgOutputReplicationOptions(PublicationName, 1);

        _logger.LogInformation("Starting logical replication");
        await foreach (var message in connection.StartReplication(slot, options, cancellationToken))
        {
            if (message is CommitMessage)
                await PublishEvents(cancellationToken);

            connection.SetReplicationStatus(message.WalEnd);
        }
    }

    private async Task PublishEvents(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisherService>();
        await eventPublisher.PublishEvents(cancellationToken);
    }

    private async Task Initialize(
        string eventLogTableName,
        CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        if (!await IsPublicationExists(connection, cancellationToken))
            await CreatePublication(connection, eventLogTableName, cancellationToken);

        if (!await IsPublicationSlotExists(connection, cancellationToken))
            await CreatePublicationSlot(connection, cancellationToken);

    }

    private async Task<bool> IsPublicationExists(NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        var checkPublicationQuery = "SELECT 1 FROM pg_publication WHERE pubname = @pub_name";

        using var checkCmd = new NpgsqlCommand(checkPublicationQuery, connection);
        checkCmd.Parameters.AddWithValue("pub_name", PublicationName);
        return await checkCmd.ExecuteScalarAsync(cancellationToken) != null;
    }

    private async Task CreatePublication(
        NpgsqlConnection connection, string eventLogTableName, CancellationToken cancellationToken)
    {
        using var createCmd = new NpgsqlCommand(
            $"CREATE PUBLICATION {PublicationName} FOR TABLE {eventLogTableName} WITH (publish = 'insert');",
            connection);

        await createCmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Created event pub table {tableName}", eventLogTableName);
    }

    private async Task<bool> IsPublicationSlotExists(NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        var checkSlotQuery = "SELECT 1 FROM pg_replication_slots WHERE slot_name = @slot_name";

        using var checkCmd = new NpgsqlCommand(checkSlotQuery, connection);
        checkCmd.Parameters.AddWithValue("slot_name", PublicationSlotName);
        return await checkCmd.ExecuteScalarAsync(cancellationToken) != null;
    }

    private async Task CreatePublicationSlot(
        NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        using var createCmd = new NpgsqlCommand(
            $"SELECT * FROM pg_create_logical_replication_slot('{PublicationSlotName}', 'pgoutput');",
            connection);

        await createCmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Created logical replication slot {slotName}", PublicationSlotName);
    }

    public static AsyncPolicy HandleSqlException(ILogger logger)
    {
        return Policy.Handle<NpgsqlException>()
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(5),
                onRetry: (exception, retry, _) =>
                {
                    logger.LogWarning(
                        exception,
                        "Exception \"{Message}\" occured on connecting to database. retry attempt {retry}",
                        exception.Message,
                        retry);
                });
    }
}
