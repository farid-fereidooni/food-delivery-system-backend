namespace RestaurantManagement.Domain.Models.Query;

public class AddressQuery(string street, string city, string state, string zipCode)
{
    public string Street { get; set; } = street;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
    public string ZipCode { get; set; } = zipCode;
}
