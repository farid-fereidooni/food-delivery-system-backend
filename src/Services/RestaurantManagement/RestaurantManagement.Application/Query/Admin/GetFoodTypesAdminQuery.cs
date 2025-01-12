using MediatR;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.Query.Admin;

public record GetFoodTypesAdminQuery : IQuery<Result<ICollection<FoodTypeQuery>>>;

public class GetFoodTypesAdminQueryHandler : IRequestHandler<GetFoodTypesAdminQuery, Result<ICollection<FoodTypeQuery>>>
{
    private readonly IFoodTypeQueryRepository _repository;

    public GetFoodTypesAdminQueryHandler(IFoodTypeQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<FoodTypeQuery>>> Handle(
        GetFoodTypesAdminQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await _repository.GetAll(cancellationToken));
    }
}
