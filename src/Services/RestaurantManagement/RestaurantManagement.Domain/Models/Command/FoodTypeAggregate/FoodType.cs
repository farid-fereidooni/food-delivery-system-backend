using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;

public class FoodType : AggregateRoot
{
    protected FoodType()
    {
    }

    public FoodType(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;

    public void Rename(string newName)
    {
        Name = newName;
    }
}
