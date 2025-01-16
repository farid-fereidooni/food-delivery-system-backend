namespace RestaurantManagement.Write.Domain.Models.MenuAggregate;

public class FoodTypeMenuItem
{
    protected FoodTypeMenuItem()
    {
    }

    public FoodTypeMenuItem(Guid foodTypeId, Guid menuItemId)
    {
        FoodTypeId = foodTypeId;
        MenuItemId = menuItemId;
    }

    public Guid FoodTypeId { get; protected set; }
    public Guid MenuItemId { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not FoodTypeMenuItem item)
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        return item.FoodTypeId == FoodTypeId && item.MenuItemId == MenuItemId;
    }

    public override int GetHashCode()
    {
        return FoodTypeId.GetHashCode() ^ MenuItemId.GetHashCode();
    }

    public static bool operator ==(FoodTypeMenuItem left, FoodTypeMenuItem right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(FoodTypeMenuItem left, FoodTypeMenuItem right)
    {
        return !(left == right);
    }
}
