using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Menus;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record IncreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public IncreaseMenuItemStockCommand ToCommand(Guid menuId, Guid menuItemId)
    {
        return new IncreaseMenuItemStockCommand(menuId, menuItemId, Amount);
    }
}
