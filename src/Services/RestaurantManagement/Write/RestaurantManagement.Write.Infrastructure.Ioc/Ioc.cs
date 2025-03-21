using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantManagement.Write.Domain.Contracts;
using DbContext = RestaurantManagement.Write.Infrastructure.Database.DbContext;


namespace RestaurantManagement.Write.Infrastructure.Ioc;

public static class Ioc
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
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

        return builder;
    }
}
