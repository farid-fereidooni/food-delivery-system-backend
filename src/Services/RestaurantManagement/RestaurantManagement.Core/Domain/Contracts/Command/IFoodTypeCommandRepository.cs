using RestaurantManagement.Core.Domain.Models.FoodTypeAggregate;

namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface IFoodTypeCommandRepository : ICommandRepository<FoodType>;
