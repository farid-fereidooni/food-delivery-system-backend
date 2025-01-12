using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.Dtos;

public class MyRestaurantDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public RestaurantStatus Status { get; set; }
    public required AddressQuery Address { get; set; }

    public static MyRestaurantDto From(RestaurantQuery item)
    {
        return new MyRestaurantDto
        {
            Id = item.Id,
            OwnerId = item.OwnerId,
            Name = item.Name,
            Status = item.Status,
            Address = item.Address,
        };
    }
}
