using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainServices;

namespace RestaurantManagement.Application.Query.Public;

public record IsUserRestaurantOwnerQuery : IQuery<bool>;

public class IsUserRestaurantOwnerQueryHandler : IRequestHandler<IsUserRestaurantOwnerQuery, bool>
{
    private readonly IRestaurantOwnerService _service;
    private readonly IAuthService _authService;

    public IsUserRestaurantOwnerQueryHandler(IRestaurantOwnerService service, IAuthService authService)
    {
        _service = service;
        _authService = authService;
    }

    public async Task<bool> Handle(IsUserRestaurantOwnerQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return false;

        return await _service.IsRestaurantOwner(currentUserResult.Unwrap(), cancellationToken);
    }
}
