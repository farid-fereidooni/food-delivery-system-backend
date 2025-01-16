using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Query.RestaurantOwners.Restaurants;

namespace RestaurantManagement.Read.Api.Controllers.RestaurantOwner;

[ApiController]
[Route("api/restaurant-owner/[controller]")]
[RestaurantOwnerAuthorize]
public class RestaurantController : Controller
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetRestaurant(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyRestaurantQuery(), cancellationToken);
        return result.ToApiResponse();
    }
}
