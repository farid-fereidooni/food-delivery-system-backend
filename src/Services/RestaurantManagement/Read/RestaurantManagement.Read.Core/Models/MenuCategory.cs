using RestaurantManagement.Read.Domain.Contracts;

namespace RestaurantManagement.Read.Domain.Models;

public class MenuCategory : StorableRoot
{
    public required Guid OwnerId { get; set; }
    public required string Name { get; set; }
}
