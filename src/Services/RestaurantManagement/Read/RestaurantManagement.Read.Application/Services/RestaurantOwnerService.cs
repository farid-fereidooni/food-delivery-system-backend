using RestaurantManagement.Read.Domain.Contracts.Repositories;

namespace RestaurantManagement.Read.Application.Services;

public interface IRestaurantOwnerService
{
    Task<bool> IsRestaurantOwner(Guid currentUserId, CancellationToken cancellationToken = default);
}

public class RestaurantOwnerService : IRestaurantOwnerService
{
    private readonly IRestaurantOwnerRepository _repository;

    public RestaurantOwnerService(IRestaurantOwnerRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> IsRestaurantOwner(Guid currentUserId, CancellationToken cancellationToken = default)
    {
        return _repository.ExistsAsync(currentUserId, cancellationToken);
    }
}
