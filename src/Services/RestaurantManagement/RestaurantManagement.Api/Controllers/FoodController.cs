using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.Food;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Application.Command.Foods;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodController : Controller
{
    private readonly IMediator _mediator;

    public FoodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFood(
        [FromBody] CreateFoodRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFood(
        [FromRoute] Guid id,
        [FromBody] UpdateFoodRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFood(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteFoodCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

}
