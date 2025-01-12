using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Application.Command.Menus;

public record AddMenuItemCommand(
    Guid CategoryId,
    string Name,
    decimal Price,
    string? Description,
    Guid[] FoodTypeIds) : ICommand<Result<EntityCreatedDto>>;

public class AddMenuItemCommandHandler : IRequestHandler<AddMenuItemCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public AddMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        AddMenuItemCommand request, CancellationToken cancellationToken)
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
