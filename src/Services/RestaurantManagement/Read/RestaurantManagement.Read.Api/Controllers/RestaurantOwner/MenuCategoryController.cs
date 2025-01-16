using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Query.RestaurantOwners.MenuCategories;

namespace RestaurantManagement.Read.Api.Controllers.RestaurantOwner;

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

    [HttpGet]
    public async Task<IActionResult> GetMenuCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyMenuCategoriesQuery(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMenuCategory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyMenuCategoryQuery(id), cancellationToken);
        return result.ToApiResponse();
    }
}
