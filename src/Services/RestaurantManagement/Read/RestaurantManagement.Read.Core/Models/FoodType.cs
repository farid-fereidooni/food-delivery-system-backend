using RestaurantManagement.Read.Domain.Contracts;

namespace RestaurantManagement.Read.Domain.Models;

public class FoodType : StorableRoot
{
    public required string Name { get; set; }
}
