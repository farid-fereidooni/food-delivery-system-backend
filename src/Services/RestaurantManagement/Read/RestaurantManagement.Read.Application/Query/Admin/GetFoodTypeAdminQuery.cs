using MediatR;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Read.Application.Query.Admin;

public record GetFoodTypeAdminQuery(Guid Id) : IQuery<Result<FoodType>>;

public class GetFoodTypeAdminQueryHandler : IRequestHandler<GetFoodTypeAdminQuery, Result<FoodType>>
{
    private readonly IFoodTypeRepository _repository;

    public GetFoodTypeAdminQueryHandler(IFoodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<FoodType>> Handle(GetFoodTypeAdminQuery request, CancellationToken cancellationToken)
    {
        var foodType = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (foodType == null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        return foodType;
    }
}
