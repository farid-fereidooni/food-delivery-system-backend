using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Query.RestaurantOwners.Menus;

namespace RestaurantManagement.Read.Api.Controllers.RestaurantOwner;

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

    [HttpGet("current/menu-items")]
    public async Task<IActionResult> GetMenus(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyMenuItemsQuery(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpGet("current/menu-items/{id:guid}")]
    public async Task<IActionResult> GetMenu(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyMenuItemQuery(id), cancellationToken);
        return result.ToApiResponse();
    }
}
