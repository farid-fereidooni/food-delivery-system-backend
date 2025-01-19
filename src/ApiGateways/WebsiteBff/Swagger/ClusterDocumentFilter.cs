using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Transforms.Builder;

namespace WebsiteBff.Swagger;

public class ClusterDocumentFilter : IDocumentFilter
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IReadOnlyDictionary<string, OperationType> _operationTypeMapping;

    private ProxyConfiguration _config;

    public ClusterDocumentFilter(
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ProxyConfiguration> configOptions,
        IEnumerable<ITransformFactory>? factories)
    {
        _config = configOptions.CurrentValue;
        _httpClientFactory = httpClientFactory;

        configOptions.OnChange(x => { _config = x; });

        _operationTypeMapping = new Dictionary<string, OperationType>
        {
            { "GET", OperationType.Get },
            { "POST", OperationType.Post },
            { "PUT", OperationType.Put },
            { "DELETE", OperationType.Delete },
            { "PATCH", OperationType.Patch },
            { "HEAD", OperationType.Head },
            { "OPTIONS", OperationType.Options },
            { "TRACE", OperationType.Trace },
        };
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (!_config.Routes.Any())
            return;

        var paths = new OpenApiPaths();
        var components = new OpenApiComponents();
        var securityRequirements = new List<OpenApiSecurityRequirement>();
        var tags = new List<OpenApiTag>();

        var pairs = _config.Routes
            .Where(route => route.Value.ClusterId == context.DocumentName)
            .Select(
                route => new ConfigPair
                {
                    Route = route,
                    Cluster = _config.Clusters.GetValueOrDefault(route.Value.ClusterId)
                })
            .Where(w => w.Cluster?.Swagger is not null);

        foreach (var config in pairs)
        {
            AddDocument(config, paths, components, securityRequirements, tags);
        }

        swaggerDoc.Info = swaggerDoc.Info;
        swaggerDoc.Paths = paths;
        swaggerDoc.SecurityRequirements = securityRequirements;
        swaggerDoc.Components = components;
        swaggerDoc.Tags = tags;
    }

    private void AddDocument(
        in ConfigPair configPair,
        OpenApiPaths paths,
        OpenApiComponents components,
        List<OpenApiSecurityRequirement> securityRequirements,
        List<OpenApiTag> tags)
    {
        var cluster = configPair.Cluster;
        ArgumentNullException.ThrowIfNull(cluster);
        ArgumentNullException.ThrowIfNull(cluster.Swagger);

        var destination = cluster.Destinations.FirstOrDefault();
        ArgumentNullException.ThrowIfNull(destination);

        if (!Uri.TryCreate(new Uri(destination.Value.Address), cluster.Swagger.Path, out var swaggerUrl))
            throw new ArgumentException("Unable to combine specified url values");

        var httpClient = _httpClientFactory.CreateClient(Constants.SwaggerHttpClient);

        var stream = httpClient.GetStreamAsync(swaggerUrl).Result;
        var doc = new OpenApiStreamReader().Read(stream, out _);

        foreach (var docPath in doc.Paths)
        {
            var docOperationKeys = docPath.Value.Operations.Keys.ToList();

            var availableMethods = configPair.Route.Value.Match.Methods;
            var availableOperations = _operationTypeMapping
                .Where(q => availableMethods.Contains(q.Key))
                .Select(q => q.Value)
                .ToList();

            foreach (var operationKey in docOperationKeys)
            {
                if (!availableOperations.Contains(operationKey))
                {
                    docPath.Value.Operations.Remove(operationKey);
                }
            }

            var transformedPath = ApplySwaggerTransformation(docPath, configPair);
            paths.TryAdd(transformedPath.Key, transformedPath.Value);
        }

        components.Add(doc.Components);
        securityRequirements.AddRange(doc.SecurityRequirements);
        tags.AddRange(doc.Tags);
    }

    private KeyValuePair<string, OpenApiPathItem> ApplySwaggerTransformation(
        in KeyValuePair<string, OpenApiPathItem> path, in ConfigPair configPair)
    {
        var transforms = configPair.Route.Value.Transforms
            .SelectMany(s => s)
            .Reverse()
            .ToArray();

        if (transforms.Length == 0)
            return path;

        var transformedPath = new KeyValuePair<string, OpenApiPathItem>(path.Key, path.Value);
        foreach (var transform in transforms)
        {
            var transformer = SwaggerTransformer.Create(transform.Key);
            if (transformer is null)
                continue;

            transformedPath = transformer.Apply(transformedPath, transform.Value);
        }

        return transformedPath;
    }
}
