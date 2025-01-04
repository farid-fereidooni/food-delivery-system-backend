using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Menus;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Exceptions;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Domain.Models.Command.MenuAggregate;

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

        AddDomainEvent(new MenuCreatedEvent(Id, restaurantId));
    }

    public Guid RestaurantId { get; private set; }

    protected internal EntityCollection<MenuItem> MenuItems { get; } = new();

    public bool IsActive { get; private set; }
    public uint Version { get; set; }

    public Result CanAddMenuItem(Guid categoryId, FoodSpecification specification, ICollection<Guid> typeIds)
    {
        if (MenuItems.Count >= MaxMenuItemLength)
            return new Error(CommonResource.Validation_MaxMenuItemReached);

        if (MenuItems.Any(x => x.CategoryId == categoryId && x.Specification.Name == specification.Name))
            return new Error(CommonResource.App_DuplicatedMenuItem);

        return Result.Success();
    }

    public Guid AddMenuItem(Guid categoryId, FoodSpecification specification, ICollection<Guid> typeIds)
    {
        var validation = CanAddMenuItem(categoryId, specification, typeIds);
        InvalidDomainStateException.ThrowIfError(validation);

        var menuItem = new MenuItem(categoryId, specification, typeIds);
        MenuItems.Add(menuItem);

        AddDomainEvent(
            new MenuItemAddedEvent(
                menuItem.Id,
                Id,
                RestaurantId,
                menuItem.CategoryId,
                menuItem.Specification.Name,
                menuItem.Specification.Price,
                menuItem.Specification.Description,
                menuItem.Stock,
                typeIds));
        return menuItem.Id;
    }

    public void UpdateMenuItemFoodSpecification(Guid menuItemId, FoodSpecification specification)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.UpdateFoodSpecification(specification);

        AddDomainEvent(
            new MenuItemUpdatedEvent(
                menuItemId,
                Id,
                RestaurantId,
                menuItem.Specification.Name,
                menuItem.Specification.Price,
                menuItem.Specification.Description));
    }

    public void SetMenuItemFoodTypes(Guid menuItemId, IEnumerable<Guid> foodTypeIds)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.SetFoodTypes(foodTypeIds);

        AddDomainEvent(new MenuItemFoodTypesUpdatedEvent(
            menuItemId,Id, RestaurantId, menuItem.FoodTypes.ToList()));
    }

    public void AddMenuItemFoodTypes(Guid menuItemId, params Guid[] foodTypeId)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.AddFoodTypes(foodTypeId);

        AddDomainEvent(new MenuItemFoodTypesUpdatedEvent(
            menuItemId, Id, RestaurantId, menuItem.FoodTypes.ToList()));
    }

    public void RemoveMenuItemFoodTypes(Guid menuItemId, params Guid[] foodTypeId)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.RemoveFoodTypes(foodTypeId);

        AddDomainEvent(new MenuItemFoodTypesUpdatedEvent(menuItemId,
            Id, RestaurantId, menuItem.FoodTypes.ToList()));
    }

    public Result CanChangeMenuItemCategory(Guid menuItemId, Guid newCategoryId)
    {
        if (!MenuItems.ContainsBydId(menuItemId))
            return new Error(CommonResource.App_MenuItemNotFound).WithReason(ErrorReason.NotFound);

        return Result.Success();
    }

    public void ChangeMenuItemCategory(Guid menuItemId, Guid newCategoryId)
    {
        InvalidDomainOperationException.ThrowIfError(CanChangeMenuItemCategory(menuItemId, newCategoryId));

        var menuItem = GetMenuItem(menuItemId);
        menuItem.ChangeCategory(newCategoryId);

        AddDomainEvent(new MenuItemCategoryUpdatedEvent(menuItemId, Id, RestaurantId, newCategoryId));
    }

    public void RemoveMenuItem(Guid menuItemId)
    {
        MenuItems.RemoveById(menuItemId);

        AddDomainEvent(new MenuItemRemovedEvent(menuItemId, Id, RestaurantId));
    }

    public Result CanAddStock(Guid menuItemId, uint number)
    {
        if (!MenuItems.ContainsBydId(menuItemId))
            return new Error(CommonResource.App_MenuItemNotFound).WithReason(ErrorReason.NotFound);

        return Result.Success();
    }

    public void AddStock(Guid menuItemId, uint amount)
    {
        InvalidDomainOperationException.ThrowIfError(CanAddStock(menuItemId, amount));

        var menuItem = GetMenuItem(menuItemId);
        menuItem.AddStock(amount);

        AddDomainEvent(new MenuItemStockUpdatedEvent(menuItemId, Id, RestaurantId, amount));
    }

    public Result CanDecreaseStock(Guid menuItemId, uint number)
    {
        var menuItem = GetMenuItem(menuItemId);
        return menuItem.CanDecreaseStock(number);
    }

    public void DecreaseStock(Guid menuItemId, uint amount)
    {
        var menuItem = GetMenuItem(menuItemId);
        menuItem.DecreaseStock(amount);

        AddDomainEvent(new MenuItemStockUpdatedEvent(menuItemId, Id, RestaurantId, amount));
    }

    private MenuItem GetMenuItem(Guid menuItemId)
    {
        return MenuItems.TryGetById(menuItemId, out var menuItem)
            ? menuItem
            : throw InvalidDomainOperationException.Create("Menu item does not exist.");
    }
}
