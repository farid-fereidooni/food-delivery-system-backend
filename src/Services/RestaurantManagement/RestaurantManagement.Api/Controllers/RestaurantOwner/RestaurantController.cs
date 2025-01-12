using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.Restaurant;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Application.Command.RestaurantOwners.Restaurants;
using RestaurantManagement.Application.Query.RestaurantOwners.Restaurants;

namespace RestaurantManagement.Api.Controllers.RestaurantOwner;

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

    [HttpGet("current")]
    public async Task<IActionResult> GetRestaurant(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyRestaurantQuery(), cancellationToken);
        return result.ToApiResponse();
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
