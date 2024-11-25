using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Core.Application.Command.RestaurantOwner;

namespace RestaurantManagement.Api.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> CreateRestaurantOwner(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateRestaurantOwnerCommand(), cancellationToken);
        return result.ToApiResponse();
    }
}
