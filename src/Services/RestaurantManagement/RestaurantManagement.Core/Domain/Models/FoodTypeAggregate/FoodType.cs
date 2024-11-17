using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;

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

    public void Rename(string newName) => Name = newName;
}
