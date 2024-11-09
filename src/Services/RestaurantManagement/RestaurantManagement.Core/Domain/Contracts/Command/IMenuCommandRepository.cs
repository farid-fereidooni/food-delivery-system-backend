using RestaurantManagement.Core.Domain.Models.MenuAggregate;

namespace RestaurantManagement.Core.Domain.Contracts.Command;

public interface IMenuCommandRepository : ICommandRepository<Menu>;
