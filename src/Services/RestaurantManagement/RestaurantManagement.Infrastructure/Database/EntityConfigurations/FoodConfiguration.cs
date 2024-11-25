using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Core.Domain.Models.FoodAggregate;
using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Infrastructure.Database.EntityConfigurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ComplexProperty(c => c.Specification);

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
