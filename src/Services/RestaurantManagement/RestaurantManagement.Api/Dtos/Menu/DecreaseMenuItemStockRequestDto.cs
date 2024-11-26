using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Menus;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record DecreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public DecreaseMenuItemStockCommand ToCommand(Guid menuId, Guid menuItemId)
    {
        return new DecreaseMenuItemStockCommand(menuId, menuItemId, Amount);
    }
}
