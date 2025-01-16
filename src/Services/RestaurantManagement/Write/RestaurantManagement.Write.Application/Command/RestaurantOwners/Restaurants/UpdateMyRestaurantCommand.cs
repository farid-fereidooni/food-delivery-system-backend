using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Resources;
using RestaurantManagement.Write.Domain.ValueObjects;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Restaurants;

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
    private readonly IRestaurantOwnerRepository _ownerRepository;
    private readonly IAuthService _authService;

    public UpdateMyRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerRepository ownerRepository,
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
