using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Domain.Contracts.Repositories;

public interface IFoodTypeRepository : IRepository<FoodType>
{
    Task<ICollection<FoodType>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken = default);
    Task<ICollection<FoodType>> GetAll(CancellationToken cancellationToken = default);
}
