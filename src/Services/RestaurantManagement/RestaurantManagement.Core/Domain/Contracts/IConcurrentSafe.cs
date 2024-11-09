namespace RestaurantManagement.Core.Domain.Contracts;

public interface IConcurrentSafe
{
    uint Version { get; set; }
}
