using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Resources;
using RestaurantManagement.Write.Domain.ValueObjects;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;

public record CreateMyRestaurantCommand(
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : ICommand<Result<EntityCreatedDto>>;

public class CreateMyRestaurantCommandHandler : IRequestHandler<CreateMyRestaurantCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerRepository _ownerRepository;
    private readonly IAuthService _authService;

    public CreateMyRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateMyRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
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
                await _unitOfWork.SaveAsync(cancellationToken);
                return EntityCreatedDto.From(restaurantId);
            });
    }
}
