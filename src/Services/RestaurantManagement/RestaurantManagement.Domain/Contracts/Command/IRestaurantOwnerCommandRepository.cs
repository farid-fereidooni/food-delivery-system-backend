using RestaurantManagement.Domain.Models.Command.RestaurantAggregate;

namespace RestaurantManagement.Domain.Contracts.Command;

public interface IRestaurantOwnerCommandRepository : ICommandRepository<RestaurantOwner>;
