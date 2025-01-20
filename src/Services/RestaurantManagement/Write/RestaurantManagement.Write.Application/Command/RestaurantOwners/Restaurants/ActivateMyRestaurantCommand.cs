using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;

public record ActivateMyRestaurantCommand(Guid Id) : ICommand<Result>;

public class ActivateMyRestaurantCommandHandler : IRequestHandler<ActivateMyRestaurantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerRepository _ownerRepository;
    private readonly IAuthService _authService;

    public ActivateMyRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(ActivateMyRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var owner = await _ownerRepository.GetByIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (owner is null)
            return new Error(CommonResource.App_YouAreNotOwner);

        owner.ActivateRestaurant(request.Id);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
