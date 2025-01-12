using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.RestaurantOwners.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record DecreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public DecreaseMyMenuItemStockCommand ToCommand(Guid menuItemId)
    {
        return new DecreaseMyMenuItemStockCommand(menuItemId, Amount);
    }
}
