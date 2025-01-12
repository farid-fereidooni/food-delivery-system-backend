using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Application.Command.RestaurantOwners.Menus;

public record UpdateMyMenuItemCommand(
    Guid MenuItemId,
    Guid CategoryId,
    string Name,
    decimal Price,
    string? Description,
    Guid[] FoodTypeIds) : ICommand<Result>;

public class UpdateMyMenuItemCommandHandler : IRequestHandler<UpdateMyMenuItemCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public UpdateMyMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(UpdateMyMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var menu = await _menuRepository.GetByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu
            .CanChangeMenuItemCategory(request.MenuItemId, request.CategoryId)
            .And(FoodSpecification.Validate(request.Name, request.Price, request.Description))
            .AndThenAsync(async () =>
            {
                menu.ChangeMenuItemCategory(request.MenuItemId, request.CategoryId);
                menu.UpdateMenuItemFoodSpecification(
                    request.MenuItemId, new FoodSpecification(request.Name, request.Price, request.Description));
                menu.SetMenuItemFoodTypes(request.MenuItemId, request.FoodTypeIds);

                await _unitOfWork.SaveAsync(cancellationToken);
            });
    }
}
