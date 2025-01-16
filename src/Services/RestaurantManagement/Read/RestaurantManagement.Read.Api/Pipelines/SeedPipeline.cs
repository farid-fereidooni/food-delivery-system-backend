using RestaurantManagement.Read.Api.Utilities;
using RestaurantManagement.Read.Infrastructure.Database;

namespace RestaurantManagement.Read.Api.Pipelines;

public static class SeedPipeline
{
    public static async Task Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var hostLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

        var queryContext = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        await PollyHelper.HandleNoSqlNotReady(logger).ExecuteAsync(async () =>
        {
            await queryContext.RunMigrationsAsync(hostLifetime.ApplicationStopping);
        });
    }
}
