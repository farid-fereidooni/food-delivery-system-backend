using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Query.RestaurantOwners.Menus;

public record GetMyMenuItemQuery(Guid Id) : IQuery<Result<RestaurantMenuItemQuery>>;

public class GetMyMenuItemQueryHandler : IRequestHandler<GetMyMenuItemQuery, Result<RestaurantMenuItemQuery>>
{
    private readonly IRestaurantQueryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuItemQueryHandler(IRestaurantQueryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<RestaurantMenuItemQuery>> Handle(
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
