using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Models.Command.MenuAggregate;

namespace RestaurantManagement.Domain.Models.Query;

public class MenuQuery : StorableRoot
{
    public Guid RestaurantId { get; set; }
    public IEnumerable<MenuItem> MenuItems { get; set; } = [];
}

public class MenuQueryMenuItem : Storable
{
    public Guid CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public Guid FoodId { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public required string Name { get; set; }
    public string[] FoodTypes { get; set; } = [];
    public uint Stock { get; set; }
}
