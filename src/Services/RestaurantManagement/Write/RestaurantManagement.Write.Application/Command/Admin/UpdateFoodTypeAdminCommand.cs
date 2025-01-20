using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.Admin;

public record UpdateFoodTypeAdminCommand(Guid Id, string Name) : ICommand<Result>;

public class UpdateFoodTypeAdminCommandHandler : IRequestHandler<UpdateFoodTypeAdminCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeRepository _foodTypeRepository;

    public UpdateFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeRepository foodTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _foodTypeRepository = foodTypeRepository;
    }

    public async Task<Result> Handle(
        UpdateFoodTypeAdminCommand request, CancellationToken cancellationToken)
    {
        var foodType = await _foodTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foodType is null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        if (await _foodTypeRepository.ExistsWithNameAsync(request.Name, request.Id, cancellationToken))
            return new Error(CommonResource.App_FoodTypeAlreadyExists);

        foodType.Rename(request.Name);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
