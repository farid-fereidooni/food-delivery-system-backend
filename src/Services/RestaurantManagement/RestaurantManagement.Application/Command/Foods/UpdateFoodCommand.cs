using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Application.Command.Foods;

public record UpdateFoodCommand(
    Guid Id,
    string Name,
    decimal Price,
    string? Description,
    ICollection<Guid> FoodTypeIds)
    : ICommand<Result>;

public class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodCommandRepository _foodRepository;
    private readonly IFoodTypeCommandRepository _foodTypeRepository;
    private readonly IAuthService _authService;

    public UpdateFoodCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodCommandRepository foodRepository,
        IFoodTypeCommandRepository foodTypeRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _foodRepository = foodRepository;
        _foodTypeRepository = foodTypeRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        var food = await _foodRepository.GetByIdAsync(request.Id, cancellationToken);
        if (food?.OwnerId != ownerId)
            return new Error(CommonResource.App_FoodNotFound).WithReason(ErrorReason.NotFound);

        var error = new Error();
        if (await _foodRepository.ExistsWithName(ownerId, request.Name, food.Id, cancellationToken))
            error.AddMessage(CommonResource.App_CategoryAlreadyExists);

        var newFoodSpecificationResult = FoodSpecification.Validate(request.Name, request.Price, request.Description)
            .AndThen(() => new FoodSpecification(request.Name, request.Price, request.Description));

        if (newFoodSpecificationResult.IsFailure)
            error.CombineError(newFoodSpecificationResult);

        if (!await _foodTypeRepository.ExistsAsync(request.FoodTypeIds, cancellationToken))
            error.AddMessage(CommonResource.App_InvalidFoodTypes);

        if (!error.IsEmpty)
            return error;

        food.SetFoodTypes(request.FoodTypeIds);
        food.UpdateFoodSpecification(newFoodSpecificationResult.Unwrap());

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Success();
    }
}
