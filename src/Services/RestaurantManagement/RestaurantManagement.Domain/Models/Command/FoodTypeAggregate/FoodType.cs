using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.FoodTypes;

namespace RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;

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
