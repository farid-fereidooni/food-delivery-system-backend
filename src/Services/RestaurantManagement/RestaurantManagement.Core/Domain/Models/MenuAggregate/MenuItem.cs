using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Exceptions;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Domain.Models.MenuAggregate;

public class MenuItem : Entity, IConcurrentSafe
{
    protected MenuItem()
    {

    }

    public MenuItem(Guid categoryId, Guid foodId)
    {
        CategoryId = categoryId;
        FoodId = foodId;
        Stock = 0;
    }

    public Guid CategoryId { get; private set; }

    public Guid MenuId { get; private set; }
    public Guid FoodId { get; private set; }

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
            return new Error(
                nameof(CommonResource.Validation_CannotDecreaseStock), CommonResource.Validation_CannotDecreaseStock);

        return Result.Success();
    }

    internal void DecreaseStock(uint number)
    {
        var validation = CanDecreaseStock(number);
        InvalidDomainStateException.ThrowIfError(validation);
        Stock -= number;
    }
}
