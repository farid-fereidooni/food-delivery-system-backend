using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace WebsiteBff.Swagger;

public static class Pipeline
{
    public static IServiceCollection AddClusterSwagger(this IServiceCollection collection, IConfigurationSection config)
    {
        var parsedConfig = config.Get<ProxyConfiguration>();
        if (parsedConfig == null)
            return collection;

        collection.AddSwaggerGen(options =>
        {
            foreach (var cluster in parsedConfig.Clusters)
            {
                var name = cluster.Key;
                options.SwaggerDoc(name, new OpenApiInfo {Title = name, Version = name});
            }

            options.DocumentFilter<ClusterDocumentFilter>();
        });

        collection.Configure<ProxyConfiguration>(config);
        collection.AddHttpClient(Constants.SwaggerHttpClient);

        return collection;
    }

    public static void UseClusterSwaggerUi(this WebApplication app, IConfigurationSection? configuration = null)
    {
        var parsedConfig = configuration != null
            ? configuration.Get<ProxyConfiguration>()
            : app.Services.GetRequiredService<IOptions<ProxyConfiguration>>().Value;

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        if (parsedConfig == null)
        {
            logger.LogError("Null Configuration provided for cluster swagger");
            return;
        }

        app.UseSwaggerUI(options =>
        {
            foreach (var cluster in parsedConfig.Clusters)
            {
                var name = cluster.Key;
                options.SwaggerEndpoint($"/swagger/{name}/swagger.json", name);
            }
        });
    }
}
