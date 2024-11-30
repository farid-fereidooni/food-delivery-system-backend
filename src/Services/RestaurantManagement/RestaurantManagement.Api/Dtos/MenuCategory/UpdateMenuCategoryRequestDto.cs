using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.MenuCategories;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.MenuCategory;

public record UpdateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateMenuCategoryCommand ToCommand(Guid id) => new(id, Name);
}
