using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Write.Api.Dtos.Menu;
using RestaurantManagement.Write.Api.Helpers;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

namespace RestaurantManagement.Write.Api.Controllers.RestaurantOwner;

[ApiController]
[Route("api/restaurant-owner/[controller]")]
[RestaurantOwnerAuthorize]
public class MenuController : Controller
{
    private readonly IMediator _mediator;

    public MenuController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("current/menu-items")]
    public async Task<IActionResult> AddMenuItem(
        [FromBody] AddMenuItemRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("current/menu-items/{id:guid}")]
    public async Task<IActionResult> UpdateMenuItem(
        [FromRoute] Guid id,
        [FromBody] UpdateMenuItemRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("current/menu-items/{id:guid}/increase-stock")]
    public async Task<IActionResult> IncreaseMenuItemStock(
        [FromRoute] Guid id,
        [FromBody] IncreaseMenuItemStockRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("current/menu-items/{id:guid}/decrease-stock")]
    public async Task<IActionResult> IncreaseMenuItemStock(
        [FromRoute] Guid id,
        [FromBody] DecreaseMenuItemStockRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpDelete("current/menu-items/{id:guid}")]
    public async Task<IActionResult> UpdateMenuItem(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteMyMenuItemCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

}
