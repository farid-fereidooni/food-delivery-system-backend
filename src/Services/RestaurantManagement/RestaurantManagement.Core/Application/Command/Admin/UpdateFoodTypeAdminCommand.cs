using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Admin;

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
