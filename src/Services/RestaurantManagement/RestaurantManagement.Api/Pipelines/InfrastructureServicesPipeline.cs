using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Infrastructure.Database;

namespace RestaurantManagement.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("CommandDatabase"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<ApplicationDbContext>());

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Repository")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime());

        builder.Services.AddHttpContextAccessor();
        return builder;
    }
}
