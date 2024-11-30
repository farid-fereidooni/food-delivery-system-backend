using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Application.Command.Foods;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Food;

public record CreateFoodRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    [NonNegative(ErrorMessage = nameof(CommonResource.Validation_FieldShouldNotBeNegative))]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public Guid[] FoodTypeIds { get; set; } = [];

    public CreateFoodCommand ToCommand() => new(Name, Price, Description, FoodTypeIds);
}
