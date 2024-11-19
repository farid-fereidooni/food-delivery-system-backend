using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Restaurant;

public record CreateRestaurantCommand(
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IRequest<Result<EntityCreatedDto>>;

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

        var address = Address.Create(request.Street, request.City, request.State, request.ZipCode);
        return await owner
            .CanAddRestaurant(request.Name, address)
            .AndThenAsync(async () =>
            {
                var restaurantId = owner.AddRestaurant(request.Name, address);
                await _unitOfWork.CommitAsync(cancellationToken);
                return EntityCreatedDto.From(restaurantId);
            });
    }
}
