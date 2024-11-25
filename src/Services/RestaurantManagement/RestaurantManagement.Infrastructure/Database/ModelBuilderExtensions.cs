using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Infrastructure.Database;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyDefaultConfigurations(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            entityType
                .ApplyPrimaryKeyConfigurations(builder)
                .ApplyConcurrencyConfigurations(builder)
                .ApplyEntityConfigurations(builder);
        }

        return builder;
    }

    private static IMutableEntityType ApplyPrimaryKeyConfigurations(
        this IMutableEntityType entityType, ModelBuilder modelBuilder)
    {
        if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
        {
            modelBuilder.Entity(entityType.ClrType).Property(nameof(Entity.Id)).ValueGeneratedNever();
        }

        return entityType;
    }

    private static IMutableEntityType ApplyEntityConfigurations(
        this IMutableEntityType entityType, ModelBuilder modelBuilder)
    {
        if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
        {
            modelBuilder.Entity(entityType.ClrType).Ignore(nameof(Entity.DomainEvents));
        }

        return entityType;
    }

    private static IMutableEntityType ApplyConcurrencyConfigurations(
        this IMutableEntityType entityType, ModelBuilder modelBuilder)
    {
        if (typeof(IConcurrentSafe).IsAssignableFrom(entityType.ClrType))
        {
            modelBuilder.Entity(entityType.ClrType).Property(nameof(IConcurrentSafe.Version)).IsRowVersion();
        }

        return entityType;
    }
}
