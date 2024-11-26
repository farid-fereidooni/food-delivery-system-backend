using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.Restaurant;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Core.Application.Command.Restaurants;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var result = await _mediator.Send(new ActivateRestaurantCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateRestaurant(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeactivateRestaurantCommand(id), cancellationToken);
        return result.ToApiResponse();
    }
}
