using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Api.Utilities;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class NonNegativeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        var numericValue = value as int?;
        return !(numericValue < 0);
    }
}
