using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.MenuCategories;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.MenuCategory;

public record UpdateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateMenuCategoryCommand ToCommand(Guid id) => new(id, Name);
}
