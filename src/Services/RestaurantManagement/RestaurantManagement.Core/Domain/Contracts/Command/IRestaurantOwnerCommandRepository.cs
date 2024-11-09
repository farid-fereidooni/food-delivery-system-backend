using RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface IRestaurantOwnerCommandRepository : ICommandRepository<RestaurantOwner>;
