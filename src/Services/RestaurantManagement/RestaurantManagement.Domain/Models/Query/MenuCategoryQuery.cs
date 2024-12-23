using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.Models.Query;

public class MenuCategoryQuery : StorableRoot
{
    public required Guid OwnerId { get; set; }
    public required string Name { get; set; }
}
