using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Foods;

public record DeleteFoodCommand(Guid Id)
    : ICommand<Result>;

public class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodCommandRepository _foodRepository;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public DeleteFoodCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodCommandRepository foodRepository,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _foodRepository = foodRepository;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        var food = await _foodRepository.GetByIdAsync(request.Id, cancellationToken);
        if (food is null)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        if (food.OwnerId != ownerId)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        if (await _menuRepository.AnyMenuItemWithFoodAsync(food.Id, cancellationToken))
            return new Error(CommonResource.App_FoodIsAlreadyUsed);

        await _foodRepository.DeleteAsync(food, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Success();
    }
}
