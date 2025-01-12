using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.Query.RestaurantOwners.MenuCategories;

public record GetMyMenuCategoriesQuery : IQuery<Result<ICollection<MenuCategoryQuery>>>;

public class GetMyMenuCategoriesQueryHandler
    : IRequestHandler<GetMyMenuCategoriesQuery, Result<ICollection<MenuCategoryQuery>>>
{
    private readonly IMenuCategoryQueryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuCategoriesQueryHandler(IMenuCategoryQueryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<ICollection<MenuCategoryQuery>>> Handle(
        GetMyMenuCategoriesQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        return Result.Success(await _repository.GetByOwnerId(currentUserResult.Unwrap(), cancellationToken));
    }
}
