using MediatR;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.Query.RestaurantOwners.MenuCategories;

public record GetMyMenuCategoriesQuery : IQuery<Result<ICollection<MenuCategory>>>;

public class GetMyMenuCategoriesQueryHandler
    : IRequestHandler<GetMyMenuCategoriesQuery, Result<ICollection<MenuCategory>>>
{
    private readonly IMenuCategoryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyMenuCategoriesQueryHandler(IMenuCategoryRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<ICollection<MenuCategory>>> Handle(
        GetMyMenuCategoriesQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        return Result.Success(await _repository.GetByOwnerId(currentUserResult.Unwrap(), cancellationToken));
    }
}
