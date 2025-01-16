namespace RestaurantManagement.Write.Domain.Contracts;

public interface IConcurrentSafe
{
    uint Version { get; set; }
}
