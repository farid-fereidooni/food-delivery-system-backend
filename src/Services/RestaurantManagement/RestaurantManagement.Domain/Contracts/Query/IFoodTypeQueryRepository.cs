using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Domain.Contracts.Query;

public interface IFoodTypeQueryRepository : IQueryRepository<FoodTypeQuery>
{
    Task<ICollection<FoodTypeQuery>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken = default);
}
