using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Write.Api.Dtos.MenuCategory;
using RestaurantManagement.Write.Api.Helpers;
using RestaurantManagement.Write.Application.Command.RestaurantOwners.MenuCategories;

namespace RestaurantManagement.Write.Api.Controllers.RestaurantOwner;

[ApiController]
[Route("api/restaurant-owner/[controller]")]
[RestaurantOwnerAuthorize]
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
        var result = await _mediator.Send(new DeleteMyMenuCategoryCommand(id), cancellationToken);
        return result.ToApiResponse();
    }

}
