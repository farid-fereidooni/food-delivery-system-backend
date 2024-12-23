using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Application.Command.Menus;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Menu;

public record AddMenuItemRequestDto
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

    public AddMenuItemCommand ToCommand()
    {
        return new AddMenuItemCommand(CategoryId, Name, Price, Description, FoodTypeIds);
    }
}
