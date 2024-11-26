using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Core.Application.Command.Foods;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.Food;

public record UpdateFoodRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    [NonNegative(ErrorMessage = nameof(CommonResource.Validation_FieldShouldNotBeNegative))]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public Guid[] FoodTypeIds { get; set; } = [];

    public UpdateFoodCommand ToCommand(Guid id) => new(id, Name, Price, Description, FoodTypeIds);
}
