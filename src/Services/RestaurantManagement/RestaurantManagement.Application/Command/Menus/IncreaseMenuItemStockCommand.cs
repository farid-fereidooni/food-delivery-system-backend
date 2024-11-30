using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.Menus;

public record IncreaseMenuItemStockCommand(Guid MenuId, Guid MenuItemId, uint Amount) : IRequest<Result>;

public class IncreaseMenuItemStockCommandHandler: IRequestHandler<IncreaseMenuItemStockCommand, Result>
{
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IConcurrencyHandler _concurrencyHandler;

    public IncreaseMenuItemStockCommandHandler(
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

    public async Task<Result> Handle(IncreaseMenuItemStockCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        return await _concurrencyHandler.RetryAsync(
            async () => await IncreaseMenuItemStock(request, ownerId, cancellationToken),
            cancellationToken);
    }

    private async Task<Result> IncreaseMenuItemStock(
        IncreaseMenuItemStockCommand request, Guid ownerId, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu
            .CanAddStock(request.MenuItemId, request.Amount)
            .AndThenAsync(async () =>
            {
                menu.AddStock(request.MenuItemId, request.Amount);
                await _unitOfWork.CommitAsync(cancellationToken);
            });
    }
}
