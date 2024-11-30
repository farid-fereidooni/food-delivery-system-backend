using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record AddMenuItemRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public Guid FoodId { get; set; }

    public AddMenuItemCommand ToCommand(Guid menuId)
    {
        return new AddMenuItemCommand(menuId, CategoryId, FoodId);
    }
}
