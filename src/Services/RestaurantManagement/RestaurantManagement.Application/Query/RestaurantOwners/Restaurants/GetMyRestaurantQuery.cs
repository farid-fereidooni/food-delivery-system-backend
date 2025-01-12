using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Application.Dtos;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Query.RestaurantOwners.Restaurants;

public record GetMyRestaurantQuery : IQuery<Result<MyRestaurantDto>>;

public class GetMyRestaurantQueryHandler : IRequestHandler<GetMyRestaurantQuery, Result<MyRestaurantDto>>
{
    private readonly IRestaurantQueryRepository _repository;
    private readonly IAuthService _authService;

    public GetMyRestaurantQueryHandler(IRestaurantQueryRepository repository, IAuthService authService)
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
