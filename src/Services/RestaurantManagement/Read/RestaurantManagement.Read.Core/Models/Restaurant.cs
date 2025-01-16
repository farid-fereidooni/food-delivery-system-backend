using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Enums;

namespace RestaurantManagement.Read.Domain.Models;

public class Restaurant : StorableRoot
{
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public RestaurantStatus Status { get; set; }
    public required Address Address { get; set; }
    public ICollection<RestaurantMenu> Menus { get; set; } = [];
}

public class RestaurantMenu : Storable
{
    public Guid RestaurantId { get; set; }
    public ICollection<RestaurantMenuItem> MenuItems { get; set; } = [];
}

public class RestaurantMenuItem : Storable
{
    public Guid CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public RestaurantFoodTypes[] FoodTypes { get; set; } = [];
    public uint Stock { get; set; }
}

public class RestaurantFoodTypes : Storable {
    public required string Name { get; set; }
}
