using RestaurantManagement.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IFoodTypeCommandRepository : ICommandRepository<FoodType>
{
    Task<bool> ExistsAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default);
}
