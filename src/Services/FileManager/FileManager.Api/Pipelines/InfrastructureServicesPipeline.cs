using FileManager.Core.Contracts;
using FileManager.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FileManager.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<AppDbContext>());

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<DbContext>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime());

        builder.Services.AddHttpContextAccessor();
        return builder;
    }
}
