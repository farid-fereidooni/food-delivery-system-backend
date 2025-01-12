using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Application.Command.RestaurantOwners.Restaurants;

public record UpdateMyRestaurantCommand(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : ICommand<Result>;

public class UpdateMyRestaurantCommandHandler : IRequestHandler<UpdateMyRestaurantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerCommandRepository _ownerRepository;
    private readonly IAuthService _authService;

    public UpdateMyRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerCommandRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(UpdateMyRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var owner = await _ownerRepository.GetByIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (owner is null)
            return new Error(CommonResource.App_YouAreNotOwner);

        var address = Address.Create(request.Street, request.City, request.State, request.ZipCode);
        owner.UpdateRestaurantInfo(request.Id, request.Name, address);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
