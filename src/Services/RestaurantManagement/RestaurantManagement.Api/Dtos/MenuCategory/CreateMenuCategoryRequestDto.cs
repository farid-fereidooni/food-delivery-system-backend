using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.RestaurantOwners.MenuCategories;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.MenuCategory;

public record CreateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public CreateMyMenuCategoryCommand ToCommand() => new(Name);
}
