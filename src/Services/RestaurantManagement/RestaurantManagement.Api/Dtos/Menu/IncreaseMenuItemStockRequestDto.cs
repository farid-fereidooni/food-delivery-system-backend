using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record IncreaseMenuItemStockRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = nameof(CommonResource.Validation_InvalidFieldValue))]
    public uint Amount { get; set; }

    public IncreaseMenuItemStockCommand ToCommand(Guid menuItemId)
    {
        return new IncreaseMenuItemStockCommand(menuItemId, Amount);
    }
}
