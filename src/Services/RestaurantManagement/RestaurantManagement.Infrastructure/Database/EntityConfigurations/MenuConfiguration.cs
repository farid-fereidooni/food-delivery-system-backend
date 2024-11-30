using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Domain.Models.MenuAggregate;
using RestaurantManagement.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database.EntityConfigurations;

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

        builder.HasIndex(i => new { i.MenuId, i.CategoryId, i.FoodId}).IsUnique();
    }
}
