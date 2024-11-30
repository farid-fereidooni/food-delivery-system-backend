using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.Admin;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.FoodType;

public record CreateFoodTypeRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateFoodTypeAdminCommand ToCommand() => new(Name);
}
