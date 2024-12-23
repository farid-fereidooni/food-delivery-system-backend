using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.MenuCategories;

public record MenuCategoryUpdatedEvent(Guid Id, string Name) : IDomainEvent;
