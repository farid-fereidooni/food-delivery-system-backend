using RestaurantManagement.Write.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Write.Domain.Contracts.Repositories;

public interface IFoodTypeRepository : IRepository<FoodType>
{
    Task<bool> ExistsAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
