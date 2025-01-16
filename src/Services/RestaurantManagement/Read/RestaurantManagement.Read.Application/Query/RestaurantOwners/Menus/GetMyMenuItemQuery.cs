using MediatR;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Domain.Resources;

namespace RestaurantManagement.Read.Application.Query.RestaurantOwners.Menus;

public record GetMyMenuItemQuery(Guid Id) : IQuery<Result<RestaurantMenuItem>>;

public class GetMyMenuItemQueryHandler : IRequestHandler<GetMyMenuItemQuery, Result<RestaurantMenuItem>>
{
    private readonly IRestaurantRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuItemQueryHandler(IRestaurantRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<RestaurantMenuItem>> Handle(
        GetMyMenuItemQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var menuItems = await _repository.GetMenuItemsByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken);
        var menuItem = menuItems.FirstOrDefault(f => f.Id == request.Id);
        if (menuItem == null)
            return new Error(CommonResource.App_MenuItemNotFound).WithReason(ErrorReason.NotFound);

        return menuItem;
    }
}
