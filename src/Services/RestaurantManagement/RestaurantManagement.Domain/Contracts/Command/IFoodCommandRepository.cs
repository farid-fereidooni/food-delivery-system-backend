using RestaurantManagement.Domain.Models.Command.FoodAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IFoodCommandRepository : ICommandRepository<Food>
{
    Task<bool> ExistsWithName(
        Guid ownerId, string name, Guid? excludeId = default, CancellationToken cancellationToken = default);
}
