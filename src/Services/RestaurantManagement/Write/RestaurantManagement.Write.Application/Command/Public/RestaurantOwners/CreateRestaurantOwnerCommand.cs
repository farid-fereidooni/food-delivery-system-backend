using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Models.RestaurantAggregate;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.Public.RestaurantOwners;

public record CreateRestaurantOwnerCommand : ICommand<Result<EntityCreatedDto>>;

public class CreateRestaurantOwnerCommandHandler
    : IRequestHandler<CreateRestaurantOwnerCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerRepository _ownerRepository;
    private readonly IAuthService _authService;

    public CreateRestaurantOwnerCommandHandler(
        IUnitOfWork unitOfWork, IRestaurantOwnerRepository ownerRepository, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateRestaurantOwnerCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var newOwnerId = currentUserResult.Unwrap();

        if (await _ownerRepository.ExistsAsync(newOwnerId, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_YouAreAlreadyOwner);

        var owner = new RestaurantOwner(newOwnerId);

        await _ownerRepository.AddAsync(owner, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return EntityCreatedDto.From(owner.Id);
    }
}
