using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Menus;

public record DeleteMenuItemCommand(Guid MenuItemId) : ICommand<Result>;

public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public DeleteMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(
        DeleteMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var menu = await _menuRepository.GetByOwnerIdAsync(currentUserResult.Unwrap(), cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        menu.RemoveMenuItem(request.MenuItemId);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
