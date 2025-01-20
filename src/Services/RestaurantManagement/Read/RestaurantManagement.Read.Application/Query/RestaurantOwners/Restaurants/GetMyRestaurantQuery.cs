using MediatR;
using RestaurantManagement.Read.Application.Dtos;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Read.Application.Query.RestaurantOwners.Restaurants;

public record GetMyRestaurantQuery : IQuery<Result<MyRestaurantDto>>;

public class GetMyRestaurantQueryHandler : IRequestHandler<GetMyRestaurantQuery, Result<MyRestaurantDto>>
{
    private readonly IRestaurantRepository _repository;
    private readonly IAuthService _authService;

    public GetMyRestaurantQueryHandler(IRestaurantRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<Result<MyRestaurantDto>> Handle(GetMyRestaurantQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var restaurant = await _repository.GetByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (restaurant == null)
            return new Error(CommonResource.App_RestaurantNotFound).WithReason(ErrorReason.NotFound);

        return MyRestaurantDto.From(restaurant);
    }
}
