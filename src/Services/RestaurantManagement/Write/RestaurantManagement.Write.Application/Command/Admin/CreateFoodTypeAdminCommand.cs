using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.Admin;

public record CreateFoodTypeAdminCommand(string Name) : ICommand<Result<EntityCreatedDto>>;

public class CreateFoodTypeAdminCommandHandler : IRequestHandler<CreateFoodTypeAdminCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeRepository _foodTypeRepository;

    public CreateFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeRepository foodTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _foodTypeRepository = foodTypeRepository;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateFoodTypeAdminCommand request, CancellationToken cancellationToken)
    {
        if (await _foodTypeRepository.ExistsWithNameAsync(request.Name, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_FoodTypeAlreadyExists);

        var foodType = new FoodType(request.Name);
        await _foodTypeRepository.AddAsync(foodType, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return EntityCreatedDto.From(foodType.Id);
    }
}
