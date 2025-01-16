using RestaurantManagement.Write.Api.Utilities;
using RestaurantManagement.Write.Infrastructure.Database;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class SeedPipeline
{
    public static async Task Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var commandContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var hostLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

        await PollyHelper.HandleSqlNotReady(logger).ExecuteAsync(async () =>
        {
            await commandContext.MigrateAsync(hostLifetime.ApplicationStopping);
        });
    }
}
