namespace RestaurantManagement.Domain.Contracts;

public interface IConcurrentSafe
{
    uint Version { get; set; }
}
