using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using RestaurantManagement.Read.Infrastructure.Database;
using DbContext = RestaurantManagement.Read.Infrastructure.Database.DbContext;

namespace RestaurantManagement.Read.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    private const string MongoDatabaseName = "RestaurantManagement";
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        var mongoSettings = MongoClientSettings.FromConnectionString(
            builder.Configuration.GetConnectionString("Database"));

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
        builder.Services.AddSingleton<DbContext>();

        builder.Services.AddTransient<IMigrationRunner, MigrationRunner>();

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<DbContext>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Repository")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IMigration>())
                .As<IMigration>()
                .WithTransientLifetime());

        builder.Services.AddHttpContextAccessor();
        return builder;
    }
}
