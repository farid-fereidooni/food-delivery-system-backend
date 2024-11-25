using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Contracts.Command;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.MenuItem;

public record AddMenuItemCommand(Guid MenuId, Guid CategoryId, Guid FoodId) : IRequest<Result<EntityCreatedDto>>;

public class AddMenuItemCommandHandler : IRequestHandler<AddMenuItemCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;
    private readonly IAuthService _authService;

    public AddMenuItemCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        AddMenuItemCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();
        var ownerId = currentUserResult.Unwrap();

        var menu = await _menuRepository.GetByIdAsync(request.MenuId, ownerId, cancellationToken);
        if (menu is null)
            return new Error(CommonResource.App_MenuNotFound);

        var itemResult = menu.CanAddMenuItem(request.CategoryId, request.FoodId)
            .And(menu.AddMenuItem(request.CategoryId, request.FoodId));

        if (itemResult.IsFailure)
            return itemResult.UnwrapError();

        await _unitOfWork.CommitAsync(cancellationToken);
        return EntityCreatedDto.From(itemResult.Unwrap());
    }
}
