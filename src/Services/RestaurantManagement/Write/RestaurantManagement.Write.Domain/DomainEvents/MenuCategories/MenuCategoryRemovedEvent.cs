using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.MenuCategories;

public record MenuCategoryRemovedEvent(Guid Id) : IDomainEvent;
