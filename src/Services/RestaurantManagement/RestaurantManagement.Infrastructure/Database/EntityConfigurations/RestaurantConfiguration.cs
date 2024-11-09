using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database.EntityConfigurations;

public class RestaurantOwnerConfiguration : IEntityTypeConfiguration<RestaurantOwner>
{
    public void Configure(EntityTypeBuilder<RestaurantOwner> builder)
    {
    }
}

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasOne<RestaurantOwner>()
            .WithMany()
            .HasForeignKey(x => x.OwnerId);

        builder.ComplexProperty(p => p.Address);
    }
}
