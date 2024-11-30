using System.Diagnostics.CodeAnalysis;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Helpers;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Domain.Models.FoodAggregate;

public class Food : AggregateRoot
{
    protected Food()
    {
        FoodTypeFoods = new HashSet<FoodTypeFood>();
    }

    public Food(FoodSpecification specification, Guid ownerId, IEnumerable<Guid> typeIds)
    {
        Specification = specification;
        OwnerId = ownerId;
        SetFoodTypes(typeIds);
    }

    private ICollection<FoodTypeFood> FoodTypeFoods { get; set; }
    public Guid OwnerId { get; private set; }

    public FoodSpecification Specification { get; private set; }

    public void UpdateFoodSpecification(FoodSpecification specification)
    {
        Specification = specification;
    }

    [MemberNotNull(nameof(FoodTypeFoods))]
    public void SetFoodTypes(IEnumerable<Guid> foodTypeId)
    {
        FoodTypeFoods = foodTypeId.Select(s => new FoodTypeFood(s, Id)).ToHashSet();
    }

    public void AddFoodTypes(params Guid[] foodTypeId)
    {
        FoodTypeFoods.AddRange(foodTypeId.Select(s => new FoodTypeFood(s, Id)));
    }

    public void RemoveFoodTypes(params Guid[] foodTypeId)
    {
        FoodTypeFoods.RemoveRange(foodTypeId.Select(s => new FoodTypeFood(s, Id)));
    }
}
