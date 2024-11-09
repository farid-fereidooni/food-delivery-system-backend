using RestaurantManagement.Core.Domain.Models.MenuCategoryAggregate;

namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface IMenuCategoryCommandRepository : ICommandRepository<MenuCategory>;
