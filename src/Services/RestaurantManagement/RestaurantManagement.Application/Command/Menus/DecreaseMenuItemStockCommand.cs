using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Menus;

public record DecreaseMenuItemStockCommand(Guid MenuId, Guid MenuItemId, uint Amount) : IRequest<Result>;

public class DecreaseMenuItemStockCommandHandler: IRequestHandler<DecreaseMenuItemStockCommand, Result>
{
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IConcurrencyHandler _concurrencyHandler;

    public DecreaseMenuItemStockCommandHandler(
        IMenuCommandRepository menuRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IConcurrencyHandler concurrencyHandler)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _concurrencyHandler = concurrencyHandler;
    }

    public async Task<Result> Handle(DecreaseMenuItemStockCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        return await _concurrencyHandler.RetryAsync(
            async () => await DecreaseMenuItemStock(request, ownerId, cancellationToken),
            cancellationToken);
    }

    private async Task<Result> DecreaseMenuItemStock(
        DecreaseMenuItemStockCommand request, Guid ownerId, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu.CanDecreaseStock(request.MenuItemId, request.Amount)
            .AndThenAsync(async () =>
            {
                menu.DecreaseStock(request.MenuItemId, request.Amount);
                await _unitOfWork.CommitAsync(cancellationToken);
            });
    }
}
