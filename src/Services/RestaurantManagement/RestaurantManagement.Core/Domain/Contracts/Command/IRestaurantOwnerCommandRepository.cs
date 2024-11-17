using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IRestaurantOwnerCommandRepository : ICommandRepository<RestaurantOwner>;
