using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.Models.Query;

public class FoodTypeQuery : StorableRoot
{
    public required string Name { get; set; }
}
