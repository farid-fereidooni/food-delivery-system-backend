using EventBus.Core;
using EventBus.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.Registration;

public static class RabbitMqRegistration
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services, string host, Action<IRabbitMqRegistrationQueueBind>? setup = null)
    {
        services.AddSingleton<IEventBus, RabbitMqEventBus>();

        var eventBusConfiguration = new EventBusConfiguration
        {
            Host = host,
            Subscriptions = new Subscriptions(),
        };

        if (setup != null)
        {
            var builder = new RabbitMqRegistrationBuilder(services);
            setup(builder);
            eventBusConfiguration.Subscriptions = builder.Subscriptions;
        }

        services.AddSingleton(eventBusConfiguration);
        return services;
    }

    public static async Task StartRabbitMq(this IHost host, CancellationToken token)
    {
        using var scope = host.Services.CreateScope();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        if (eventBus is not RabbitMqEventBus)
            throw new Exception("A RabbitMq event bus must be provided");

        await eventBus.StartConsumeAsync();
    }
}
