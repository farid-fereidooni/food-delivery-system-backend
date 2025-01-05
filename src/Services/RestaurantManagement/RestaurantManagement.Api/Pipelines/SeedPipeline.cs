using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Infrastructure.Database.Command;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Api.Pipelines;

public static class SeedPipeline
{
    public static async Task Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var commandContext = scope.ServiceProvider.GetRequiredService<CommandDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var hostLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

        await PollyHelper.HandleSqlNotReady(logger).ExecuteAsync(async () =>
        {
            await commandContext.MigrateAsync(hostLifetime.ApplicationStopping);
        });

        var queryContext = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        await PollyHelper.HandleNoSqlNotReady(logger).ExecuteAsync(async () =>
        {
            await queryContext.RunMigrationsAsync(hostLifetime.ApplicationStopping);
        });
    }
}
