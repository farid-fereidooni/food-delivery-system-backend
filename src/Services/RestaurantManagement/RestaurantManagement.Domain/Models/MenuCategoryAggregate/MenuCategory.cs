using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.Models.MenuCategoryAggregate;

public class MenuCategory : AggregateRoot
{
    protected MenuCategory()
    {
    }

    public MenuCategory(Guid ownerId, string name)
    {
        OwnerId = ownerId;
        Name = name;
    }

    public void Rename(string newName) => Name = newName;

    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = null!;
}
