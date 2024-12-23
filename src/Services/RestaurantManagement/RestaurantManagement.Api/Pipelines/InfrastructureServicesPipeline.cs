using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Infrastructure.Database.Command;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CommandDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("CommandDatabase"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<CommandDbContext>());

        builder.Services.AddSingleton<IMongoClient, MongoClient>(
            _ => new MongoClient(builder.Configuration.GetConnectionString("queryDatabase")));
        builder.Services.AddSingleton<QueryDbContext>();

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<CommandDbContext>()
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
