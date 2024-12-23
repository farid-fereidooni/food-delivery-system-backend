using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Application.Command.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record UpdateMenuItemRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    [NonNegative(ErrorMessage = nameof(CommonResource.Validation_FieldShouldNotBeNegative))]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public Guid[] FoodTypeIds { get; set; } = [];

    public UpdateMenuItemCommand ToCommand(Guid menuItemId)
    {
        return new UpdateMenuItemCommand(menuItemId, CategoryId, Name, Price, Description, FoodTypeIds);
    }
}
