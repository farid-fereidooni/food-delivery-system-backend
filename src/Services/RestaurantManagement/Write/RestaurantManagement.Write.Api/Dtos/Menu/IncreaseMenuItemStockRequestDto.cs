using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Api.Dtos.Menu;

public record IncreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public IncreaseMyMenuItemStockCommand ToCommand(Guid menuItemId)
    {
        return new IncreaseMyMenuItemStockCommand(menuItemId, Amount);
    }
}
