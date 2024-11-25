using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Core.Application.Command.Restaurant;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Dtos.Restaurant;

public record UpdateRestaurantRequestDto
{
    [Required(ErrorMessage = nameof(CommonResource.Validation_FieldIsRequired))]
    public Guid Id { get; set; }

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

    public UpdateRestaurantCommand ToCommand()
    {
        return new UpdateRestaurantCommand(Id, Name, Street, City, State, ZipCode);
    }
}
