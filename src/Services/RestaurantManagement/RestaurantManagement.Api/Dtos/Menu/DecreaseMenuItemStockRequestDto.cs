using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record DecreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public DecreaseMenuItemStockCommand ToCommand(Guid menuItemId)
    {
        return new DecreaseMenuItemStockCommand(menuItemId, Amount);
    }
}
