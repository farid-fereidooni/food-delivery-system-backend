using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.MenuCategories;

public record MenuCategoryCreatedEvent(Guid Id, Guid OwnerId, string Name) : IDomainEvent;
