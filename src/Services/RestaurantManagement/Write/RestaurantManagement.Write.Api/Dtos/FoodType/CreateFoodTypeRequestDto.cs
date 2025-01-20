using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.Admin;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Api.Dtos.FoodType;

public record CreateFoodTypeRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateFoodTypeAdminCommand ToCommand() => new(Name);
}
