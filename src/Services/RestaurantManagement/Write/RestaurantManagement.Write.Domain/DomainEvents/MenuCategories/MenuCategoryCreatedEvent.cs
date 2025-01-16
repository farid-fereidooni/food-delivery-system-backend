using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.MenuCategories;

public record MenuCategoryCreatedEvent(Guid Id, Guid OwnerId, string Name) : IDomainEvent;
