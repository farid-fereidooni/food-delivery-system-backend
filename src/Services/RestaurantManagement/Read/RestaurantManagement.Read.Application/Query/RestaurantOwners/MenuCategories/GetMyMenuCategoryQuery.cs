using MediatR;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Read.Domain.Resources;

namespace RestaurantManagement.Read.Application.Query.RestaurantOwners.MenuCategories;

public record GetMyMenuCategoryQuery(Guid Id) : IQuery<Result<MenuCategory>>;

public class GetMyMenuCategoryQueryHandler : IRequestHandler<GetMyMenuCategoryQuery, Result<MenuCategory>>
{
    private readonly IMenuCategoryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuCategoryQueryHandler(IMenuCategoryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<MenuCategory>> Handle(
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
