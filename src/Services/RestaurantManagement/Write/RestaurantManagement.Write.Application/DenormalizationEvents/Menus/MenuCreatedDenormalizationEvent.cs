using EventBus.Core;

namespace RestaurantManagement.Write.Application.DenormalizationEvents.Menus;

public record MenuCreatedDenormalizationEvent(Guid Id, Guid RestaurantId) : Event;
