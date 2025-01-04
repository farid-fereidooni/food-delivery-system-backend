using EventBus.Core;

namespace RestaurantManagement.Application.DenormalizationEvents.Menus;

public record MenuCreatedDenormalizationEvent(Guid Id, Guid RestaurantId) : Event;
