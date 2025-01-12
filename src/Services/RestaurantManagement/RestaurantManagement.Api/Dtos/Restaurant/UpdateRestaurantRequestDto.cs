using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.RestaurantOwners.Restaurants;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Restaurant;

public record UpdateRestaurantRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Name { get; init; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string Street { get; init; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string City { get; init; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required string State { get; init; }

    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public required  string ZipCode { get; init; }

    public UpdateMyRestaurantCommand ToCommand(Guid id)
    {
        return new UpdateMyRestaurantCommand(id, Name, Street, City, State, ZipCode);
    }
}
