using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<DbContext>());

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<DbContext>()
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
