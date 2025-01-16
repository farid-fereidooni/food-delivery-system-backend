using EventBus.Core;

namespace RestaurantManagement.Read.Application.DenormalizationEvents.Menus;

public record MenuCreatedDenormalizationEvent(Guid Id, Guid RestaurantId) : Event;
