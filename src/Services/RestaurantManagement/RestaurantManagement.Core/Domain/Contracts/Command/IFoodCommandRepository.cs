using RestaurantManagement.Core.Domain.Models.FoodAggregate;

namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface IFoodCommandRepository : ICommandRepository<Food>;
