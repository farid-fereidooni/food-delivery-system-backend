using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Exceptions;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Domain.ValueObjects;

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

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string? Description { get; private set; }

    public static Result Validate(string name, decimal price, string? description = null)
    {
        if (price < 0)
            return new Error(CommonResource.Validation_PriceShouldNotBeNegative);

        return Result.Success();
    }

    public static Result<FoodSpecification> TryCreate(string name, decimal price, string? description = null)
    {
        return Validate(name, price, description)
            .AndThen(() => new FoodSpecification(name, price, description));
    }
}
