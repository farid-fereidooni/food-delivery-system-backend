using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Models.FoodAggregate;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Foods;

public record CreateFoodCommand(
    string Name,
    decimal Price,
    string? Description,
    ICollection<Guid> FoodTypeIds)
    : IRequest<Result<EntityCreatedDto>>;

public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodCommandRepository _foodRepository;
    private readonly IAuthService _authService;

    public CreateFoodCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodCommandRepository foodRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _foodRepository = foodRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        if (await _foodRepository.ExistsWithName(ownerId, request.Name, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_CategoryAlreadyExists);

        return await FoodSpecification.Validate(request.Name, request.Price, request.Description)
            .AndThenAsync(async () =>
            {
                var food = new Food(
                    new FoodSpecification(request.Name, request.Price, request.Description),
                    ownerId,
                    request.FoodTypeIds);

                await _foodRepository.AddAsync(food, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return EntityCreatedDto.From(food.Id);
            });
    }
}
