using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Core.Domain.Models.MenuAggregate;
using RestaurantManagement.Core.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database.EntityConfigurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.Ignore("_menuItemDictionary");
        builder.HasMany(typeof(MenuItem), "MenuItems");

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
