using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.MenuCategories;

public record UpdateMenuCategoryCommand(Guid Id, string Name) : IRequest<Result>;

public class UpdateMenuCategoryCommandHandler : IRequestHandler<UpdateMenuCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCategoryCommandRepository _categoryRepository;
    private readonly IAuthService _authService;

    public UpdateMenuCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCategoryCommandRepository categoryRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _authService = authService;
    }

    public async Task<Result> Handle(
        UpdateMenuCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null || category.OwnerId != ownerId)
            return new Error(CommonResource.App_CategoryNotFound);

        if (await _categoryRepository.ExistsWithNameAsync(
                ownerId, request.Name, request.Id, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_CategoryAlreadyExists);

        category.Rename(request.Name);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
