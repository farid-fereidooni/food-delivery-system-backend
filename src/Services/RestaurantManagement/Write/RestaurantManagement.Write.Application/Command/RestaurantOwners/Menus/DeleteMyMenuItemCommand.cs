using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Domain.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

public record DeleteMyMenuItemCommand(Guid MenuItemId) : ICommand<Result>;

public class DeleteMyMenuItemCommandHandler : IRequestHandler<DeleteMyMenuItemCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuRepository _menuRepository;
    private readonly IAuthService _authService;

    public DeleteMyMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(DeleteMyMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
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
