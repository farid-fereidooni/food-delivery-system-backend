using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Application.Command.Public.RestaurantOwners;
using RestaurantManagement.Application.Query.Public;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Api.Controllers.Public;

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

    [HttpPost]
    public async Task<IActionResult> CreateRestaurantOwner(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateRestaurantOwnerCommand(), cancellationToken);
        return result.ToApiResponse();
    }
}
