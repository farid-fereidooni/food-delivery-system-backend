using MediatR;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.Query.RestaurantOwners.Menus;

public record GetMyMenuItemsQuery : IQuery<Result<ICollection<RestaurantMenuItem>>>;

public class GetMenusRestaurantOwnerQueryHandler
    : IRequestHandler<GetMyMenuItemsQuery, Result<ICollection<RestaurantMenuItem>>>
{
    private readonly IRestaurantRepository _repository;
    private readonly IAuthService _authService;

    public GetMenusRestaurantOwnerQueryHandler(
        IRestaurantRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<ICollection<RestaurantMenuItem>>> Handle(
        GetMyMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        return Result.Success(
            await _repository.GetMenuItemsByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken));
    }
}
