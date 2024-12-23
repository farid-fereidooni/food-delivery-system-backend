using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Command.FoodTypeAggregate;

namespace RestaurantManagement.Application.Command.Admin;

public record CreateFoodTypeAdminCommand(string Name) : ICommand<Result<EntityCreatedDto>>;

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
        await _unitOfWork.SaveAsync(cancellationToken);

        return EntityCreatedDto.From(foodType.Id);
    }
}
