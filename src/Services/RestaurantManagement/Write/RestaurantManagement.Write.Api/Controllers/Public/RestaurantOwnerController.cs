using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Write.Api.Helpers;
using RestaurantManagement.Write.Application.Command.Public.RestaurantOwners;

namespace RestaurantManagement.Write.Api.Controllers.Public;

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
