using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Enums;

namespace RestaurantManagement.Domain.Models.Query;

public class RestaurantQuery : StorableRoot
{
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public RestaurantStatus Status { get; set; }
    public required AddressQuery Address { get; set; }
    public ICollection<RestaurantMenuQuery> Menus { get; set; } = [];
}

public class RestaurantMenuQuery : Storable
{
    public Guid RestaurantId { get; set; }
    public ICollection<RestaurantMenuItemQuery> MenuItems { get; set; } = [];
}

public class RestaurantMenuItemQuery : Storable
{
    public Guid CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public RestaurantFoodTypesQuery[] FoodTypes { get; set; } = [];
    public uint Stock { get; set; }
}

public class RestaurantFoodTypesQuery : Storable {
    public required string Name { get; set; }
}
