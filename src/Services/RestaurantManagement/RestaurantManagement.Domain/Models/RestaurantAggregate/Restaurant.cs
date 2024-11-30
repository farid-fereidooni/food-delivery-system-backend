using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Domain.Models.RestaurantAggregate;

public class Restaurant : Entity
{
    protected Restaurant()
    {
        Name = string.Empty;
        Address = Address.Empty;
    }

    public Restaurant(string name, Address address, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        OwnerId = ownerId;

        Status = RestaurantStatus.Inactive;

        AddDomainEvent(new RestaurantCreatedEvent(Id));
    }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Guid OwnerId { get; private set; }
    public RestaurantStatus Status { get; private set; }

    public void UpdateInfo(string newName, Address newAddress)
    {
        ArgumentNullException.ThrowIfNull(newName);
        ArgumentNullException.ThrowIfNull(newAddress);

        Name = newName;
        Address = newAddress;
    }

    public void Activate()
    {
        Status = RestaurantStatus.Active;
    }

    public void Deactivate()
    {
        Status = RestaurantStatus.Inactive;
    }

    public void Close()
    {
        Status = RestaurantStatus.Closed;
    }
}
