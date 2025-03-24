using EventBus.Registration;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Dtos;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;


namespace RestaurantManagement.Write.Worker;

public static class Ioc
{
    public static IHostApplicationBuilder AddEventBus(this IHostApplicationBuilder builder)
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

    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<DbContext>());

        return builder;
    }
}
