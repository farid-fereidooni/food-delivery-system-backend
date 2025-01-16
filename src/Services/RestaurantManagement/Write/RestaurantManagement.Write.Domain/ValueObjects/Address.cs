namespace RestaurantManagement.Write.Domain.ValueObjects;

public record Address(string Street, string City, string State, string ZipCode)
{
    public static Address Empty => new("", "", "", "");

    public static Address Create(string street, string city, string state, string zipCode)
    {
        return new Address(street.Trim(), city.Trim(), state.Trim(), zipCode.Trim());
    }
}
