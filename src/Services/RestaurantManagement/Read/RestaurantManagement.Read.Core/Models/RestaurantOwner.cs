using RestaurantManagement.Read.Domain.Contracts;

namespace RestaurantManagement.Read.Domain.Models;

public class RestaurantOwner : StorableRoot
{
    public RestaurantOwner(Guid id)
    {
        Id = id;
    }
}
