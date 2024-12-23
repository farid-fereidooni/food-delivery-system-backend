using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Domain.Contracts.Query;

public interface IMenuQueryRepository : IQueryRepository<MenuQuery>
{
    Task<ICollection<MenuQuery>> GetByRestaurantIdAsync(Guid restaurantId);
}
