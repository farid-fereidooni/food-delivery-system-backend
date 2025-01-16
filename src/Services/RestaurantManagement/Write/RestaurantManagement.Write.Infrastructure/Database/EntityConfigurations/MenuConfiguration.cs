using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Write.Domain.Models.MenuAggregate;
using RestaurantManagement.Write.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Write.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Write.Infrastructure.Database.EntityConfigurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasMany(m => m.MenuItems);

        builder.HasOne<Restaurant>()
            .WithMany()
            .HasForeignKey(f => f.RestaurantId);
    }
}

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasOne<MenuCategory>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.Ignore(m => m.FoodTypes);

        builder.ComplexProperty(c => c.Specification);

        builder.HasMany<FoodType>()
            .WithMany()
            .UsingEntity<FoodTypeMenuItem>(
                l => l.HasOne<FoodType>()
                    .WithMany()
                    .HasForeignKey(f => f.FoodTypeId)
                    .OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne<MenuItem>()
                    .WithMany(m => m.FoodTypeMenuItems)
                    .HasForeignKey(f => f.MenuItemId)
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
