using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.DomainEvents.Restaurants;

public record RestaurantActivatedEvent(Guid Id) : IDomainEvent;
