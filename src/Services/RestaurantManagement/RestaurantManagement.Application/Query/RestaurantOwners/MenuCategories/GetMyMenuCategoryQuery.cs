using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Query.RestaurantOwners.MenuCategories;

public record GetMyMenuCategoryQuery(Guid Id) : IQuery<Result<MenuCategoryQuery>>;

public class GetMyMenuCategoryQueryHandler : IRequestHandler<GetMyMenuCategoryQuery, Result<MenuCategoryQuery>>
{
    private readonly IMenuCategoryQueryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuCategoryQueryHandler(IMenuCategoryQueryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<MenuCategoryQuery>> Handle(
        GetMyMenuCategoryQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var menuCategory = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (menuCategory == null || menuCategory.OwnerId != currentUserResult.Unwrap())
            return new Error(CommonResource.App_CategoryNotFound).WithReason(ErrorReason.NotFound);

        return menuCategory;
    }
}
