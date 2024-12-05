using EventBus.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.Registration;

public static class RabbitMqRegistration
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services, Action<IRabbitMqRegistrationQueueBind> setup)
    {
        services.AddSingleton<IEventBus, RabbitMqEventBus>();

        var builder = new RabbitMqRegistrationBuilder(services);
        setup(builder);

        services.AddSingleton(builder.Subscriptions);
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
