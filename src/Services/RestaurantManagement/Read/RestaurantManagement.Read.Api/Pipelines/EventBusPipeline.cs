using EventBus.Registration;
using RestaurantManagement.Read.Application.DenormalizationEventHandlers;
using RestaurantManagement.Read.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Read.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Read.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Read.Domain.Dtos;

namespace RestaurantManagement.Read.Api.Pipelines;

public static class EventBusPipeline
{
    public static WebApplicationBuilder AddEventBus(this WebApplicationBuilder builder)
    {
        var eventBusConfiguration = builder.Configuration.GetSection(nameof(EventBusConfiguration));
        builder.Services.Configure<EventBusConfiguration>(eventBusConfiguration);

        var host = eventBusConfiguration[nameof(EventBusConfiguration.Host)];
        if (string.IsNullOrEmpty(host))
            throw new InvalidOperationException("EventBus configuration section is missing or invalid");

        var denormalizationBroker = eventBusConfiguration[nameof(EventBusConfiguration.DenormalizationBroker)]!;
        var denormalizationQueue = eventBusConfiguration[nameof(EventBusConfiguration.DenormalizationQueue)]!;

        builder.Services.AddRabbitMq(host, setup => setup
            .AddSubscription<MenuCategoryCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<MenuCategoryDenormalizationHandler>()
            .AddSubscription<MenuCategoryUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<MenuCategoryDenormalizationHandler>()
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<FoodTypeCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<FoodTypeDenormalizationHandler>()
            .AddSubscription<FoodTypeUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<FoodTypeDenormalizationHandler>()
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuItemAddedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuItemCategoryUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuItemFoodTypesUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuItemRemovedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<MenuItemStockUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<RestaurantCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<RestaurantUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<RestaurantActivatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<RestaurantDeactivatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantDenormalizationHandler>()
            .AddSubscription<RestaurantOwnerCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<RestaurantOwnerDenormalizationHandler>());

        return builder;
    }
}
