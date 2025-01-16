using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Api.Utilities;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Api.Dtos.Menu;

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

    public AddMyMenuItemCommand ToCommand()
    {
        return new AddMyMenuItemCommand(CategoryId, Name, Price, Description, FoodTypeIds);
    }
}
