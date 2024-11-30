namespace RestaurantManagement.Domain.Models.FoodAggregate;

public class FoodTypeFood
{
    protected FoodTypeFood()
    {
    }

    public FoodTypeFood(Guid foodTypeId, Guid foodId)
    {
        FoodTypeId = foodTypeId;
        FoodId = foodId;
    }

    public Guid FoodTypeId { get; protected set; }
    public Guid FoodId { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not FoodTypeFood item)
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        return item.FoodTypeId == FoodTypeId && item.FoodId == FoodId;
    }

    public override int GetHashCode()
    {
        return FoodTypeId.GetHashCode() ^ FoodId.GetHashCode();
    }

    public static bool operator ==(FoodTypeFood left, FoodTypeFood right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(FoodTypeFood left, FoodTypeFood right)
    {
        return !(left == right);
    }
}
