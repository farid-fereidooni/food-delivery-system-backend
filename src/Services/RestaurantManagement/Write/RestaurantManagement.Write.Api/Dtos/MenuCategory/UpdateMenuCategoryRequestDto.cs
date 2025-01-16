using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.MenuCategories;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Api.Dtos.MenuCategory;

public record UpdateMenuCategoryRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; set; }

    public UpdateMyMenuCategoryCommand ToCommand(Guid id) => new(id, Name);
}
