using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.RestaurantOwners.MenuCategories;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.MenuCategory;

public record UpdateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateMyMenuCategoryCommand ToCommand(Guid id) => new(id, Name);
}
