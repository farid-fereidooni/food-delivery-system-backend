using System.Diagnostics.CodeAnalysis;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Exceptions;
using RestaurantManagement.Write.Domain.Helpers;
using RestaurantManagement.Write.Domain.Resources;
using RestaurantManagement.Write.Domain.ValueObjects;

namespace RestaurantManagement.Write.Domain.Models.MenuAggregate;

public class MenuItem : Entity, IConcurrentSafe
{
    protected MenuItem()
    {
        FoodTypeMenuItems = new HashSet<FoodTypeMenuItem>();
    }

    public MenuItem(Guid categoryId, FoodSpecification specification, IEnumerable<Guid> typeIds)
    {
        CategoryId = categoryId;
        Specification = specification;
        SetFoodTypes(typeIds);
        Stock = 0;
    }

    public Guid CategoryId { get; private set; }

    public Guid MenuId { get; private set; }

    internal ICollection<FoodTypeMenuItem> FoodTypeMenuItems { get; set; }
    public IEnumerable<Guid> FoodTypes => FoodTypeMenuItems.Select(x => x.FoodTypeId);

    public FoodSpecification Specification { get; private set; }

    public uint Stock { get; private set; }
    public uint Version { get; set; }

    public bool IsAvailable => Stock > 0;

    internal void AddStock(uint number)
    {
        Stock += number;
    }

    public void ChangeCategory(Guid newCategoryId) => CategoryId = newCategoryId;

    public Result CanDecreaseStock(uint number)
    {
        if (Stock < number)
            return new Error(CommonResource.Validation_CannotDecreaseStock);

        return Result.Success();
    }

    internal void DecreaseStock(uint number)
    {
        var validation = CanDecreaseStock(number);
        InvalidDomainStateException.ThrowIfError(validation);
        Stock -= number;
    }

    public void UpdateFoodSpecification(FoodSpecification specification)
    {
        Specification = specification;
    }

    [MemberNotNull(nameof(FoodTypeMenuItems))]
    public void SetFoodTypes(IEnumerable<Guid> foodTypeId)
    {
        var newItems = foodTypeId.Select(s => new FoodTypeMenuItem(s, Id)).ToHashSet();

        FoodTypeMenuItems ??= new HashSet<FoodTypeMenuItem>();
        FoodTypeMenuItems.RemoveRange(FoodTypeMenuItems.Except(newItems));
        FoodTypeMenuItems.AddRange(newItems.Except(FoodTypeMenuItems));
    }

    public void AddFoodTypes(params Guid[] foodTypeId)
    {
        FoodTypeMenuItems.AddRange(foodTypeId.Select(s => new FoodTypeMenuItem(s, Id)));
    }

    public void RemoveFoodTypes(params Guid[] foodTypeId)
    {
        FoodTypeMenuItems.RemoveRange(foodTypeId.Select(s => new FoodTypeMenuItem(s, Id)));
    }
}
