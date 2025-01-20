using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;
using RestaurantManagement.Write.Domain.ValueObjects;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

public record AddMyMenuItemCommand(
    Guid CategoryId,
    string Name,
    decimal Price,
    string? Description,
    Guid[] FoodTypeIds) : ICommand<Result<EntityCreatedDto>>;

public class AddMyMenuItemCommandHandler : IRequestHandler<AddMyMenuItemCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuRepository _menuRepository;
    private readonly IAuthService _authService;

    public AddMyMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(AddMyMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        var menu = await _menuRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        var specificationResult = FoodSpecification.TryCreate(request.Name, request.Price, request.Description);
        if (specificationResult.IsFailure)
            return specificationResult.UnwrapError();

        var itemResult = menu.CanAddMenuItem(request.CategoryId, specificationResult.Unwrap(), request.FoodTypeIds)
            .AndThen(() => menu.AddMenuItem(request.CategoryId, specificationResult.Unwrap(), request.FoodTypeIds));
        if (itemResult.IsFailure)
            return itemResult.UnwrapError();

        await _unitOfWork.SaveAsync(cancellationToken);
        return EntityCreatedDto.From(itemResult.Unwrap());
    }
}
