using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.Admin;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Api.Dtos.FoodType;

public record UpdateFoodTypeRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateFoodTypeAdminCommand ToCommand(Guid id) => new(id, Name);
}
