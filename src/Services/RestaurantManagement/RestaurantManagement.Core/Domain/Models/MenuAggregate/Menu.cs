using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Exceptions;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Domain.Models.MenuAggregate;

public class Menu : AggregateRoot, IConcurrentSafe
{
    private const int MaxMenuItemLength = 100;
    protected Menu()
    {

    }

    public Menu(Guid restaurantId)
    {
        RestaurantId = restaurantId;
        IsActive = true;
    }

    public Guid RestaurantId { get; private set; }

    private EntityCollection<MenuItem> MenuItems { get; } = new();

    public bool IsActive { get; private set; }
    public uint Version { get; set; }

    public Result CanAddMenuItem(Guid categoryId, Guid foodId)
    {
        if (MenuItems.Count >= MaxMenuItemLength)
            return new Error(CommonResource.Validation_MaxMenuItemReached);

        if (MenuItems.Any(x => x.CategoryId == categoryId && x.FoodId == foodId))
            return new Error(CommonResource.App_DuplicatedMenuItem);

        return Result.Success();
    }

    public Guid AddMenuItem(Guid categoryId, Guid foodId)
    {
        var validation = CanAddMenuItem(categoryId, foodId);
        InvalidDomainStateException.ThrowIfError(validation);

        var menuItem = new MenuItem(categoryId, foodId);
        return menuItem.Id;
    }

    public void ChangeMenuItemCategory(Guid menuItemId, Guid newCategoryId)
    {
       var menuItem = GetMenuItem(menuItemId);
       menuItem.ChangeCategory(newCategoryId);
    }

    public void RemoveMenuItem(Guid menuItemId)
    {
        MenuItems.RemoveById(menuItemId);
    }

    public void AddStock(Guid menuItemId, uint number)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.AddStock(number);
    }

    public Result CanDecreaseStock(Guid menuItemId, uint number)
    {
        var menuItem = GetMenuItem(menuItemId);
        return menuItem.CanDecreaseStock(number);
    }

    public void DecreaseStock(Guid menuItemId, uint number)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.DecreaseStock(number);
    }

    private MenuItem GetMenuItem(Guid menuItemId)
    {
        return MenuItems.TryGetById(menuItemId, out var menuItem)
            ? menuItem
            : throw InvalidDomainOperationException.Create("Menu item does not exist.");
    }
}
