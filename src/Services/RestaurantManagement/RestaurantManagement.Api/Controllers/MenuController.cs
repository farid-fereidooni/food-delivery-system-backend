using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.Menu;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Application.Command.Menus;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var result = await _mediator.Send(new DeleteMenuItemCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

}
