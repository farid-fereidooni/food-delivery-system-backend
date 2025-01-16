using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Api.Dtos.Restaurant;

public record CreateRestaurantRequestDto
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

    public CreateMyRestaurantCommand ToCommand()
    {
        return new CreateMyRestaurantCommand(Name, Street, City, State, ZipCode);
    }
}
