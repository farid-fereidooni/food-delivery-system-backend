using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.DomainEvents.FoodTypes;

namespace RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;

public class FoodType : AggregateRoot
{
    protected FoodType()
    {
    }

    public FoodType(string name)
    {
        Name = name;
        AddDomainEvent(new FoodTypeCreatedEvent(Id, name));
    }

    public string Name { get; private set; } = null!;

    public void Rename(string newName)
    {
        Name = newName;
        AddDomainEvent(new FoodTypeUpdatedEvent(Id, Name));
    }
}
