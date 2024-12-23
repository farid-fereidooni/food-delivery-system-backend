using RestaurantManagement.Api.Utilities;
using RestaurantManagement.Infrastructure.Database.Command;

namespace RestaurantManagement.Api.Pipelines;

public static class SeedPipeline
{
    public static async Task Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CommandDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var hostLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

        await PollyHelper.HandleSqlNotReady(logger).ExecuteAsync(async () =>
        {
            await context.MigrateAsync(hostLifetime.ApplicationStopping);
        });
    }
}
