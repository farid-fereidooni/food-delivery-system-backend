using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.FoodTypes;

public record FoodTypeUpdatedEvent(Guid Id, string Name) : IDomainEvent;
