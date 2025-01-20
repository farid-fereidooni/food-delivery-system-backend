using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

public record IncreaseMyMenuItemStockCommand(Guid MenuItemId, uint Amount) : ICommand<Result>;

public class IncreaseMyMenuItemStockCommandHandler : IRequestHandler<IncreaseMyMenuItemStockCommand, Result>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IConcurrencyHandler _concurrencyHandler;

    public IncreaseMyMenuItemStockCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IConcurrencyHandler concurrencyHandler)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _concurrencyHandler = concurrencyHandler;
    }

    public async Task<Result> Handle(IncreaseMyMenuItemStockCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        return await _concurrencyHandler.RetryAsync(
            async () => await IncreaseMenuItemStock(request, ownerId, cancellationToken),
            cancellationToken);
    }

    private async Task<Result> IncreaseMenuItemStock(
        IncreaseMyMenuItemStockCommand request, Guid ownerId, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu
            .CanAddStock(request.MenuItemId, request.Amount)
            .AndThenAsync(async () =>
            {
                menu.AddStock(request.MenuItemId, request.Amount);
                await _unitOfWork.SaveAsync(cancellationToken);
            });
    }
}
