using MediatR;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Dtos;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.Query.Admin;

public record GetFoodTypesAdminQuery : IQuery<Result<ICollection<FoodType>>>;

public class GetFoodTypesAdminQueryHandler : IRequestHandler<GetFoodTypesAdminQuery, Result<ICollection<FoodType>>>
{
    private readonly IFoodTypeRepository _repository;

    public GetFoodTypesAdminQueryHandler(IFoodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<FoodType>>> Handle(
        GetFoodTypesAdminQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await _repository.GetAll(cancellationToken));
    }
}
