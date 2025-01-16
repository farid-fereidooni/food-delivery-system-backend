using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Query.Public;
using RestaurantManagement.Read.Domain.Dtos;

namespace RestaurantManagement.Read.Api.Controllers.Public;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantOwnerController : Controller
{
    private readonly IMediator _mediator;

    public RestaurantOwnerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("is-users-restaurant-owner")]
    public async Task<IActionResult> IsUserRestaurantOwner(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new IsUserRestaurantOwnerQuery(), cancellationToken);
        return Result.Success(result).ToApiResponse();
    }
}
