using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Exceptions;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Domain.ValueObjects;

public record struct FoodSpecification : IValueObject
{
    public FoodSpecification(string name, decimal price, string? description = null)
    {
        InvalidDomainStateException.ThrowIfError(Validate(name, price, description));
        ArgumentNullException.ThrowIfNull(name);
        Name = name.Trim();
        Price = price;
        Description = description?.Trim();
    }

    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string Name { get; private set; }

    public static Result Validate(string name, decimal price, string? description = null)
    {
        if (price < 0)
            return new Error(CommonResource.Validation_PriceShouldNotBeNegative);

        return Result.Success();
    }
}
