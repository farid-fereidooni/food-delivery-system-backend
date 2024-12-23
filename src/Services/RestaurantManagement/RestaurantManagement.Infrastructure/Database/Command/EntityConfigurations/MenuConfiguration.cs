using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;
using RestaurantManagement.Domain.Models.Command.MenuAggregate;
using RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate;
using RestaurantManagement.Domain.Models.Command.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database.Command.EntityConfigurations;

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
