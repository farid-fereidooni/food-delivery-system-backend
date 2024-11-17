using RestaurantManagement.Core.Domain.Models.FoodAggregate;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IFoodCommandRepository : ICommandRepository<Food>
{
    Task<bool> ExistsWithName(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default);
}
