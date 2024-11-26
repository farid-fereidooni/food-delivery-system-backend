using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.FoodType;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Core.Application.Command.Admin;

namespace RestaurantManagement.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class FoodTypeController : Controller
{
    private readonly IMediator _mediator;

    public FoodTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFoodType(
        [FromBody] CreateFoodTypeRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFoodType(
        [FromRoute] Guid id,
        [FromBody] UpdateFoodTypeRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFoodType(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteFoodTypeAdminCommand(id), cancellationToken);
        return result.ToApiResponse();
    }
}