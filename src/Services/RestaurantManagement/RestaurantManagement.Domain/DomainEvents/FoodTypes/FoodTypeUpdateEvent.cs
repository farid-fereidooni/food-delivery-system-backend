using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.FoodTypes;

public record FoodTypeUpdateEvent(Guid Id, string Name) : IDomainEvent;
