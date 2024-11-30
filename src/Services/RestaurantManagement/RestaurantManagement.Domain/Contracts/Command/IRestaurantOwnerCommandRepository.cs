using RestaurantManagement.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IRestaurantOwnerCommandRepository : ICommandRepository<RestaurantOwner>;
