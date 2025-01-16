using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Query.Admin;

namespace RestaurantManagement.Read.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Constants.AdminRole)]
public class FoodTypeController : Controller
{
    private readonly IMediator _mediator;

    public FoodTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetFoodTypes(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFoodTypesAdminQuery(), cancellationToken);
        return result.ToApiResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFoodType(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFoodTypeAdminQuery(id), cancellationToken);
        return result.ToApiResponse();
    }
}
