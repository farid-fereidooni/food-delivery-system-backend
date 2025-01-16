using EventBus.Logging;
using EventBus.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Exceptions;
using RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Write.Domain.Models.MenuAggregate;
using RestaurantManagement.Write.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Write.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Write.Infrastructure.Database;

public class DbContext(DbContextOptions options, IMediator _mediator)
    : Microsoft.EntityFrameworkCore.DbContext(options), IUnitOfWork, IEventLogUnitOfWork
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantOwner> RestaurantOwners { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<FoodType> FoodTypes { get; set; }
    public DbSet<EventLog> EventLogs { get; set; }

    private IDbContextTransaction? _currentTransaction = null;

    public Guid? CurrentTransactionId => _currentTransaction?.TransactionId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
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
        await DispatchDomainEventsAsync();

        ChangeTracker.AutoDetectChangesEnabled = true;

        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            ChangeTracker.Clear();
            throw new UpdateConcurrencyException();
        }
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

    private async Task DispatchDomainEventsAsync()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            await Database.MigrateAsync(cancellationToken);
    }
}
