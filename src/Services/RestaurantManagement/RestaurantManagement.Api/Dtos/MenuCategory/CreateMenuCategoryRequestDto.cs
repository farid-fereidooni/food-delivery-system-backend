using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.MenuCategories;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.MenuCategory;

public record CreateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateMenuCategoryCommand ToCommand() => new(Name);
}
