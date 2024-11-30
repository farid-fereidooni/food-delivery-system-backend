using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Dtos.Menu;
using RestaurantManagement.Api.Dtos.MenuCategory;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Application.Command.MenuCategories;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuCategoryController : Controller
{
    private readonly IMediator _mediator;

    public MenuCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMenuCategory(
        [FromBody] CreateMenuCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMenuCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateMenuCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMenuCategory(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteMenuCategoryCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

}
