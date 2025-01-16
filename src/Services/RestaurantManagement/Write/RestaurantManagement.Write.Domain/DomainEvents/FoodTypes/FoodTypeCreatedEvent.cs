using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.FoodTypes;

public record FoodTypeCreatedEvent(Guid Id, string Name) : IDomainEvent;
