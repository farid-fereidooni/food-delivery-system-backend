using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Query.Admin;

public record GetFoodTypeAdminQuery(Guid Id) : IQuery<Result<FoodTypeQuery>>;

public class GetFoodTypeAdminQueryHandler : IRequestHandler<GetFoodTypeAdminQuery, Result<FoodTypeQuery>>
{
    private readonly IFoodTypeQueryRepository _repository;

    public GetFoodTypeAdminQueryHandler(IFoodTypeQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<FoodTypeQuery>> Handle(GetFoodTypeAdminQuery request, CancellationToken cancellationToken)
    {
        var foodType = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (foodType == null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        return foodType;
    }
}
