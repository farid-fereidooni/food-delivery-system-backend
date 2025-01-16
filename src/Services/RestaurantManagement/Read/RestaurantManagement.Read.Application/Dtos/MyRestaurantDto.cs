using RestaurantManagement.Read.Domain.Enums;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.Dtos;

public class MyRestaurantDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public RestaurantStatus Status { get; set; }
    public required Address Address { get; set; }

    public static MyRestaurantDto From(Restaurant item)
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
