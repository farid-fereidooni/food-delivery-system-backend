using EventBus.Registration;
using RestaurantManagement.Write.Domain.Dtos;
using RestaurantManagement.Write.Infrastructure.Database;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class EventBusPipeline
{
    public static WebApplicationBuilder AddEventBus(this WebApplicationBuilder builder)
    {
        var eventBusConfiguration = builder.Configuration.GetSection(nameof(EventBusConfiguration));
        builder.Services.Configure<EventBusConfiguration>(eventBusConfiguration);

        var host = eventBusConfiguration[nameof(EventBusConfiguration.Host)];
        if (string.IsNullOrEmpty(host))
            throw new InvalidOperationException("EventBus configuration section is missing or invalid");

        builder.Services.AddRabbitMq(host, setup => setup
            .AddEventLogService<DbContext>());

        return builder;
    }
}
