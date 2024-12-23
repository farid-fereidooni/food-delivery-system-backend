using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Menus;

public record UpdateMenuItemCommand(Guid MenuId, Guid MenuItemId, Guid CategoryId) : ICommand<Result>;

public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public UpdateMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(
        UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var menu = await _menuRepository.GetByIdAsync(request.MenuId, currentUserResult.Unwrap(), cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu
            .CanChangeMenuItemCategory(request.MenuItemId, request.CategoryId)
            .AndThenAsync(async () =>
            {
                menu.ChangeMenuItemCategory(request.MenuItemId, request.CategoryId);
                await _unitOfWork.SaveAsync(cancellationToken);
            });
    }
}
