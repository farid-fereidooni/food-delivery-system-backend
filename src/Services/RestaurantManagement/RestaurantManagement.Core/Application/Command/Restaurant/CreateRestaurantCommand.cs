using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Restaurant;

public record CreateRestaurantCommand(string Name, Address Address) : IRequest<Result<EntityCreatedDto>>;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerCommandRepository _ownerRepository;
    private readonly IAuthService _authService;

    public CreateRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerCommandRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = await _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var owner = await _ownerRepository.GetByIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (owner is null)
            return new Error(CommonResource.App_YouAreNotOwner);

        return await owner
            .CanAddRestaurant(request.Name, request.Address)
            .AndThenAsync(async () =>
            {
                var restaurantId = owner.AddRestaurant(request.Name, request.Address);
                await _unitOfWork.CommitAsync(cancellationToken);
                return EntityCreatedDto.From(restaurantId);
            });
    }
}
