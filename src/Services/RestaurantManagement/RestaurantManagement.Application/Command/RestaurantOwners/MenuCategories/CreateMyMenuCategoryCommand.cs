using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Application.Command.RestaurantOwners.MenuCategories;

public record CreateMyMenuCategoryCommand(string Name) : ICommand<Result<EntityCreatedDto>>;

public class CreateMyMenuCategoryCommandHandler : IRequestHandler<CreateMyMenuCategoryCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCategoryCommandRepository _categoryRepository;
    private readonly IAuthService _authService;

    public CreateMyMenuCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMenuCategoryCommandRepository categoryRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateMyMenuCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.GetCurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        if (await _categoryRepository.ExistsWithNameAsync(ownerId, request.Name, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_CategoryAlreadyExists);

        var category = new MenuCategory(ownerId, request.Name);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return EntityCreatedDto.From(category.Id);
    }
}
