using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.FoodTypes;

public record FoodTypeRemovedEvent(Guid Id) : IDomainEvent;
