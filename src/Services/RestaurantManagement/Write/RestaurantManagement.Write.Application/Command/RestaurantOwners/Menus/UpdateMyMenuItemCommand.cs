using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Resources;
using RestaurantManagement.Write.Domain.ValueObjects;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

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
    private readonly IMenuRepository _menuRepository;
    private readonly IAuthService _authService;

    public UpdateMyMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuRepository menuRepository,
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
