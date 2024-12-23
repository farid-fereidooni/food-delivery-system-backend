using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.Models.Query;

public class FoodQuery : StorableRoot
{
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public required string Name { get; set; }
}
