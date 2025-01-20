using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.Menus;

public record DecreaseMyMenuItemStockCommand(Guid MenuItemId, uint Amount) : ICommand<Result>;

public class DecreaseMyMenuItemStockCommandHandler : IRequestHandler<DecreaseMyMenuItemStockCommand, Result>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IConcurrencyHandler _concurrencyHandler;

    public DecreaseMyMenuItemStockCommandHandler(
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

    public async Task<Result> Handle(DecreaseMyMenuItemStockCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        return await _concurrencyHandler.RetryAsync(
            async () => await DecreaseMenuItemStock(request, ownerId, cancellationToken),
            cancellationToken);
    }

    private async Task<Result> DecreaseMenuItemStock(
        DecreaseMyMenuItemStockCommand request, Guid ownerId, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        return await menu.CanDecreaseStock(request.MenuItemId, request.Amount)
            .AndThenAsync(async () =>
            {
                menu.DecreaseStock(request.MenuItemId, request.Amount);
                await _unitOfWork.SaveAsync(cancellationToken);
            });
    }
}
