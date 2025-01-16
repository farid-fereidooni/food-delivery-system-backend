namespace RestaurantManagement.Read.Domain.Models;

public class Address(string street, string city, string state, string zipCode)
{
    public string Street { get; set; } = street;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
    public string ZipCode { get; set; } = zipCode;
}
