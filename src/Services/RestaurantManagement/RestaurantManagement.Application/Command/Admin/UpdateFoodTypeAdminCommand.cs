using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Admin;

public record UpdateFoodTypeAdminCommand(Guid Id, string Name) : IRequest<Result>;

public class UpdateFoodTypeAdminCommandHandler : IRequestHandler<UpdateFoodTypeAdminCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodTypeCommandRepository _foodTypeRepository;

    public UpdateFoodTypeAdminCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodTypeCommandRepository foodTypeRepository)
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

        foodType.Rename(request.Name);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
