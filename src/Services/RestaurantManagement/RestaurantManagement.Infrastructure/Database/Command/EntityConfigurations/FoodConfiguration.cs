using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Domain.Models.Command.FoodAggregate;
using RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;
using RestaurantManagement.Domain.Models.Command.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database.Command.EntityConfigurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ComplexProperty(c => c.Specification);

        builder.HasOne<RestaurantOwner>()
            .WithMany()
            .HasForeignKey(f => f.OwnerId);

        builder.HasMany<FoodType>()
            .WithMany()
            .UsingEntity<FoodTypeFood>(
                l => l.HasOne<FoodType>().WithMany().HasForeignKey(f => f.FoodTypeId).OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne<Food>()
                    .WithMany("FoodTypeFoods")
                    .HasForeignKey(f => f.FoodId)
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
