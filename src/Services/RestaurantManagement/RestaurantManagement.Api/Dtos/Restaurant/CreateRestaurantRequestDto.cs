using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Application.Command.Restaurants;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Dtos.Restaurant;

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

    public CreateRestaurantCommand ToCommand()
    {
        return new CreateRestaurantCommand(Name, Street, City, State, ZipCode);
    }
}
