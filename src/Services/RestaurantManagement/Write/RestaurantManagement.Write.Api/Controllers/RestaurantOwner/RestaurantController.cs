using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Write.Api.Dtos.Restaurant;
using RestaurantManagement.Write.Api.Helpers;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;

namespace RestaurantManagement.Write.Api.Controllers.RestaurantOwner;

[ApiController]
[Route("api/restaurant-owner/[controller]")]
[RestaurantOwnerAuthorize]
public class RestaurantController : Controller
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(
        [FromBody]CreateRestaurantRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRestaurant(
        [FromRoute]Guid id,
        [FromBody]UpdateRestaurantRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> ActivateRestaurant(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActivateMyRestaurantCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateRestaurant(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeactivateMyRestaurantCommand(id), cancellationToken);
        return result.ToApiResponse();
    }
}
