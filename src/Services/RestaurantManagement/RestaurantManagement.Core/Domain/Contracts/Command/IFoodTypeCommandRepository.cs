using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IFoodTypeCommandRepository : ICommandRepository<FoodType>
{
    Task<bool> ExistsAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default);
}
