using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Admin;

public record DeleteFoodTypeAdminCommand(Guid Id) : ICommand<Result>;

public class DeleteFoodTypeAdminCommandHandler : IRequestHandler<DeleteFoodTypeAdminCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeCommandRepository _foodTypeRepository;

    public DeleteFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeCommandRepository foodTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _foodTypeRepository = foodTypeRepository;
    }

    public async Task<Result> Handle(
        DeleteFoodTypeAdminCommand request, CancellationToken cancellationToken)
    {
        var foodType = await _foodTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foodType is null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        await _foodTypeRepository.DeleteAsync(foodType, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
