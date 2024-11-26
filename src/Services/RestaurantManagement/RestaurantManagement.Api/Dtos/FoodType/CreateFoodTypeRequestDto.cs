using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Admin;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.FoodType;

public record CreateFoodTypeRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateFoodTypeAdminCommand ToCommand() => new(Name);
}
