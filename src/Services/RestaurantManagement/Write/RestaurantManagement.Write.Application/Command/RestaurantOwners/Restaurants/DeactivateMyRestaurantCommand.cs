using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;

public record DeactivateMyRestaurantCommand(Guid Id) : ICommand<Result>;

public class DeactivateMyRestaurantCommandHandler : IRequestHandler<DeactivateMyRestaurantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerRepository _ownerRepository;
    private readonly IAuthService _authService;

    public DeactivateMyRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(DeactivateMyRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var owner = await _ownerRepository.GetByIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (owner is null)
            return new Error(CommonResource.App_YouAreNotOwner);

        owner.DeactivateRestaurant(request.Id);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
