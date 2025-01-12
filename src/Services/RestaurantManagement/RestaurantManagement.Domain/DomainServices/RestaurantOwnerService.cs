using RestaurantManagement.Domain.Contracts.Command;

namespace RestaurantManagement.Domain.DomainServices;

public interface IRestaurantOwnerService
{
    Task<bool> IsRestaurantOwner(Guid currentUserId, CancellationToken cancellationToken = default);
}

public class RestaurantOwnerService : IRestaurantOwnerService
{
    private readonly IRestaurantOwnerCommandRepository _repository;

    public RestaurantOwnerService(IRestaurantOwnerCommandRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> IsRestaurantOwner(Guid currentUserId, CancellationToken cancellationToken = default)
    {
        return _repository.ExistsAsync(currentUserId, cancellationToken);
    }
}
