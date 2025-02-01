using FileManager.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileManager.Infrastructure.Database;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyDefaultConfigurations(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            entityType.ApplyPrimaryKeyConfigurations(builder);
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
}
