using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.MenuCategories;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Api.Dtos.MenuCategory;

public record CreateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateMyMenuCategoryCommand ToCommand() => new(Name);
}
