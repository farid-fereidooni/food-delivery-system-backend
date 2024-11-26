using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Admin;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.FoodType;

public record UpdateFoodTypeRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateFoodTypeAdminCommand ToCommand(Guid id) => new(id, Name);
}
