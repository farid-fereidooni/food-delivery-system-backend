using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.MenuCategories;

namespace RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate;

public class MenuCategory : AggregateRoot
{
    protected MenuCategory()
    {
    }

    public MenuCategory(Guid ownerId, string name)
    {
        OwnerId = ownerId;
        Name = name;
        AddDomainEvent(new MenuCategoryCreatedEvent(Id, OwnerId, name));
    }

    public void Rename(string newName)
    {
        Name = newName;
        AddDomainEvent(new MenuCategoryUpdatedEvent(Id, newName));
    }

    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = null!;
}
