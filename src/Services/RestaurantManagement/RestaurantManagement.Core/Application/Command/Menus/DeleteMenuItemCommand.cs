using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Contracts.Command;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Menus;

public record DeleteMenuItemCommand(Guid MenuId, Guid MenuItemId) : IRequest<Result>;

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

        var menu = await _menuRepository.GetByIdAsync(request.MenuId, currentUserResult.Unwrap(), cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        menu.RemoveMenuItem(request.MenuItemId);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
