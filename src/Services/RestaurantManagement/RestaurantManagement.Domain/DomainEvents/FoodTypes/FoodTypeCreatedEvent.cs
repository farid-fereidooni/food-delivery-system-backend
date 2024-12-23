using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.FoodTypes;

public record FoodTypeCreatedEvent(Guid Id, string Name) : IDomainEvent;
