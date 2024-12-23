using EventBus.Registration;
using RestaurantManagement.Application.DenormalizationEventHandlers;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Api.Pipelines;

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
            .AddEventLogService<CommandDbContext>()
            .AddSubscription<MenuCategoryCreatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<MenuCategoryCreatedDenormalizationHandler>()
            .AddSubscription<MenuCategoryUpdatedDenormalizationEvent>(denormalizationBroker, denormalizationQueue)
                .AddHandler<MenuCategoryUpdatedDenormalizationHandler>());

        return builder;
    }
}
