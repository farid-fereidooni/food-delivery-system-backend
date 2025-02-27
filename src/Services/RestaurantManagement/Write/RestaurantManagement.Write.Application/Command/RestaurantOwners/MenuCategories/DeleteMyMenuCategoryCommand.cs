using MediatR;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Application.Command.RestaurantOwners.MenuCategories;

public record DeleteMyMenuCategoryCommand(Guid Id) : ICommand<Result>;

public class DeleteMyMenuCategoryCommandHandler : IRequestHandler<DeleteMyMenuCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCategoryRepository _categoryRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IAuthService _authService;

    public DeleteMyMenuCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCategoryRepository categoryRepository,
        IMenuRepository menuRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _menuRepository = menuRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(DeleteMyMenuCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null || category.OwnerId != ownerId)
            return new Error(CommonResource.App_CategoryNotFound);

        if (await _menuRepository.AnyMenuItemWithCategoryAsync(request.Id, cancellationToken))
            return new Error(CommonResource.App_MenuCategoryUsed);

        category.HandleRemoval();

        await _categoryRepository.DeleteAsync(category, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
