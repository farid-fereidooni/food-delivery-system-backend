using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.Query.RestaurantOwners.Menus;

public record GetMyMenuItemsQuery : IQuery<Result<ICollection<RestaurantMenuItemQuery>>>;

public class GetMenusRestaurantOwnerQueryHandler
    : IRequestHandler<GetMyMenuItemsQuery, Result<ICollection<RestaurantMenuItemQuery>>>
{
    private readonly IRestaurantQueryRepository _repository;
    private readonly IAuthService _authService;

    public GetMenusRestaurantOwnerQueryHandler(
        IRestaurantQueryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<ICollection<RestaurantMenuItemQuery>>> Handle(
        GetMyMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        return Result.Success(
            await _repository.GetMenuItemsByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken));
    }
}
