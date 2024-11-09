using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Exceptions;
using RestaurantManagement.Core.Domain.Models.FoodAggregate;
using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Core.Domain.Models.MenuAggregate;
using RestaurantManagement.Core.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions options, IMediator _mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<RestaurantOwner> RestaurantOwners { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodType> FoodTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyDefaultConfigurations();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamps();
        await DispatchDomainEventsAsync();

        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new UpdateConcurrencyException();
        }
    }

    private void UpdateTimeStamps()
    {
        var now = DateTime.UtcNow;
        ChangeTracker.AutoDetectChangesEnabled = false;
        foreach (var entity in ChangeTracker.Entries<Entity>())
        {
            if (entity.State == EntityState.Added)
                entity.Entity.CreatedAt = now;

            if (entity.State == EntityState.Modified)
                entity.Entity.UpdatedAt = now;
        }
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0);

        foreach (var domainEntity in domainEntities)
        {
            foreach (var domainEvent in domainEntity.Entity.DomainEvents)
                await _mediator.Publish(domainEvent);

            domainEntity.Entity.ClearDomainEvents();
        }
    }
}
