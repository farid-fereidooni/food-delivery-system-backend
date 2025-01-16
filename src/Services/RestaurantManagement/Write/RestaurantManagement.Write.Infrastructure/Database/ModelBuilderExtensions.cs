using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Infrastructure.Database;

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

    public static void IsEnum<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        => propertyBuilder.HasConversion<string>();

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
        if (typeof(AggregateRoot).IsAssignableFrom(entityType.ClrType))
        {
            modelBuilder.Entity(entityType.ClrType).Ignore(nameof(AggregateRoot.DomainEvents));
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
