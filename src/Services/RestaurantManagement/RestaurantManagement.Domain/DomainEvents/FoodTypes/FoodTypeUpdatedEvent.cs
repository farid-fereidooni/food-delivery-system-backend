using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.FoodTypes;

public record FoodTypeUpdatedEvent(Guid Id, string Name) : IDomainEvent;
