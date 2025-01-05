using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Infrastructure.Database.Command;
using RestaurantManagement.Infrastructure.Database.Query;

namespace RestaurantManagement.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    private const string MongoDatabaseName = "RestaurantManagement";
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CommandDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("CommandDatabase"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<CommandDbContext>());

        var mongoSettings = MongoClientSettings.FromConnectionString(
            builder.Configuration.GetConnectionString("queryDatabase"));

        if (builder.Environment.IsDevelopment())
        {
            var loggerFactory = LoggerFactory.Create(b =>
            {
                b.AddSimpleConsole();
                b.SetMinimumLevel(LogLevel.Debug);
            });
            mongoSettings.LoggingSettings = new LoggingSettings(loggerFactory);
        }

        builder.Services.AddSingleton<IMongoDatabase>(
            _ => new MongoClient(mongoSettings).GetDatabase(MongoDatabaseName));
        builder.Services.AddSingleton<QueryDbContext>();

        builder.Services.AddTransient<IMigrationRunner, MigrationRunner>();

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<CommandDbContext>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Repository")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IQueryMigration>())
                .As<IQueryMigration>()
                .WithTransientLifetime());

        builder.Services.AddHttpContextAccessor();
        return builder;
    }
}
