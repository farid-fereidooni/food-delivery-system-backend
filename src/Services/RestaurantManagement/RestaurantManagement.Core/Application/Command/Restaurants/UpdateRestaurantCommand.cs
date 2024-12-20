using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Restaurants;

public record UpdateRestaurantCommand(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode) : IRequest<Result>;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerCommandRepository _ownerRepository;
    private readonly IAuthService _authService;

    public UpdateRestaurantCommandHandler(
        IUnitOfWork unitOfWork,
        IRestaurantOwnerCommandRepository ownerRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(
        UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var owner = await _ownerRepository.GetByIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (owner is null)
            return new Error(CommonResource.App_YouAreNotOwner);

        var address = Address.Create(request.Street, request.City, request.State, request.ZipCode);
        owner.UpdateRestaurantInfo(request.Id, request.Name, address);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
