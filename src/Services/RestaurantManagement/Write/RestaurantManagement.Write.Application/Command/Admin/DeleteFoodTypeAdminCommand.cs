using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.Admin;

public record DeleteFoodTypeAdminCommand(Guid Id) : ICommand<Result>;

public class DeleteFoodTypeAdminCommandHandler : IRequestHandler<DeleteFoodTypeAdminCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeRepository _foodTypeRepository;
    private readonly IMenuRepository _menuRepository;

    public DeleteFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeRepository foodTypeRepository,
        IMenuRepository menuRepository)
    {
        _unitOfWork = unitOfWork;
        _foodTypeRepository = foodTypeRepository;
        _menuRepository = menuRepository;
    }

    public async Task<Result> Handle(
        DeleteFoodTypeAdminCommand request, CancellationToken cancellationToken)
    {
        var foodType = await _foodTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foodType is null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        if (await _menuRepository.AnyMenuItemWithFoodTypeAsync(foodType.Id, cancellationToken))
            return new Error(CommonResource.App_FoodTypeUsed);

        foodType.HandleRemoval();
        await _foodTypeRepository.DeleteAsync(foodType, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
