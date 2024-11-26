using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.MenuCategories;

public record CreateMenuCategoryCommand(string Name) : IRequest<Result<EntityCreatedDto>>;

public class CreateMenuCategoryCommandHandler : IRequestHandler<CreateMenuCategoryCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCategoryCommandRepository _categoryRepository;
    private readonly IAuthService _authService;

    public CreateMenuCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCategoryCommandRepository categoryRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateMenuCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        if (await _categoryRepository.ExistsWithNameAsync(ownerId, request.Name, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_CategoryAlreadyExists);

        var category = new Domain.Models.MenuCategoryAggregate.MenuCategory(ownerId, request.Name);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return EntityCreatedDto.From(category.Id);
    }
}
