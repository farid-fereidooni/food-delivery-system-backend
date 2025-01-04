using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Domain.DomainEvents.Restaurants;

public record RestaurantActivatedEvent(Guid Id) : IDomainEvent;
