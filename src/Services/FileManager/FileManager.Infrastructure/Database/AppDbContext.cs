using EventBus.Logging;
using EventBus.Models;
using FileManager.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using File = FileManager.Core.Models.File;

namespace FileManager.Infrastructure.Database;

public class AppDbContext(DbContextOptions options) : DbContext(options), IEventLogUnitOfWork
{
    public DbSet<File> Files { get; set; }
    public DbSet<EventLog> EventLogs { get; set; }

    private IDbContextTransaction? _currentTransaction = null;

    public Guid? CurrentTransactionId => _currentTransaction?.TransactionId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyDefaultConfigurations();

        modelBuilder.AddEventLogs();
    }

    public async ValueTask<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Already started transaction");

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction.TransactionId;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        ChangeTracker.AutoDetectChangesEnabled = false;

        UpdateTimeStamps();

        ChangeTracker.AutoDetectChangesEnabled = true;

        await SaveChangesAsync(cancellationToken);
    }

    public async ValueTask CommitTransaction(Guid transactionId, CancellationToken cancellationToken = default)
    {
        if (_currentTransaction?.TransactionId != transactionId)
            throw new InvalidOperationException("Invalid transaction ID");

        try
        {
            await SaveAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _currentTransaction = null;
        }
    }

    private void UpdateTimeStamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entity in ChangeTracker.Entries<Entity>())
        {
            if (entity.State == EntityState.Added)
                entity.Entity.CreatedAt = now;

            if (entity.State == EntityState.Modified)
                entity.Entity.UpdatedAt = now;
        }
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            await Database.MigrateAsync(cancellationToken);
    }
}
