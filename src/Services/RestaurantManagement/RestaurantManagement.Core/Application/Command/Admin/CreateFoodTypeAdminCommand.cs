using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Core.Application.Command.Admin;

public record CreateFoodTypeAdminCommand(string Name) : IRequest<Result<EntityCreatedDto>>;

public class CreateFoodTypeAdminCommandHandler : IRequestHandler<CreateFoodTypeAdminCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeCommandRepository _foodTypeRepository;

    public CreateFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeCommandRepository foodTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _foodTypeRepository = foodTypeRepository;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateFoodTypeAdminCommand request, CancellationToken cancellationToken)
    {
        var foodType = new FoodType(request.Name);

        await _foodTypeRepository.AddAsync(foodType, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return EntityCreatedDto.From(foodType.Id);
    }
}
