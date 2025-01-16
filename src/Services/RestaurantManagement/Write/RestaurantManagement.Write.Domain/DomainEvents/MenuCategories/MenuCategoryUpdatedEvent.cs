using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.MenuCategories;

public record MenuCategoryUpdatedEvent(Guid Id, string Name) : IDomainEvent;
