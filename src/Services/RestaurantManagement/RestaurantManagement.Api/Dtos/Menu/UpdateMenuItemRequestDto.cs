using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Menus;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record UpdateMenuItemRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public Guid CategoryId { get; set; }

    public UpdateMenuItemCommand ToCommand(Guid menuId, Guid menuItemId)
    {
        return new UpdateMenuItemCommand(menuId, menuItemId, CategoryId);
    }
}
