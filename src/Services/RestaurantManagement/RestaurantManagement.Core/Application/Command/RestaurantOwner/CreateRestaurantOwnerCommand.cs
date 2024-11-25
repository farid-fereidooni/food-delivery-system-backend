using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.RestaurantOwner;

public record CreateRestaurantOwnerCommand : IRequest<Result<EntityCreatedDto>>;

public class CreateRestaurantOwnerCommandHandler
    : IRequestHandler<CreateRestaurantOwnerCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantOwnerCommandRepository _ownerRepository;
    private readonly IAuthService _authService;

    public CreateRestaurantOwnerCommandHandler(
        IUnitOfWork unitOfWork, IRestaurantOwnerCommandRepository ownerRepository, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _ownerRepository = ownerRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateRestaurantOwnerCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var newOwnerId = currentUserResult.Unwrap();

        if (await _ownerRepository.ExistsAsync(newOwnerId, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_YouAreAlreadyOwner);

        var owner = new Domain.Models.RestaurantAggregate.RestaurantOwner(newOwnerId);

        await _ownerRepository.AddAsync(owner, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return EntityCreatedDto.From(owner.Id);
    }
}
