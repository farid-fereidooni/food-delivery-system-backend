namespace RestaurantManagement.Core.Domain.ValueObjects;

public record Address(string Street, string City, string State, string ZipCode)
{
    public static Address Empty => new("", "", "", "");
}
