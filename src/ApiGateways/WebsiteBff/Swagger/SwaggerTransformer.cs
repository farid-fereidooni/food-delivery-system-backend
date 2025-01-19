using Microsoft.OpenApi.Models;

namespace WebsiteBff.Swagger;

public abstract class SwaggerTransformer
{
    public abstract KeyValuePair<string, OpenApiPathItem> Apply(
        in KeyValuePair<string, OpenApiPathItem> path, string transformValue);

    public static SwaggerTransformer? Create(string transformKey)
        => transformKey switch
        {
            Constants.PathRemovePrefix => new PathRemovePrefixSwaggerTransformer(),
            Constants.PathPrefix => new PathPrefixSwaggerTransformer(),
            _ => null
        };
}

public class PathRemovePrefixSwaggerTransformer : SwaggerTransformer
{
    public override KeyValuePair<string, OpenApiPathItem> Apply(
        in KeyValuePair<string, OpenApiPathItem> path, string transformValue)
    {
        var newPathKey = $"{transformValue.TrimEnd('/')}/{path.Key.TrimStart('/')}";
        return new KeyValuePair<string, OpenApiPathItem>(newPathKey, path.Value);
    }
}

public class PathPrefixSwaggerTransformer : SwaggerTransformer
{
    public override KeyValuePair<string, OpenApiPathItem> Apply(
        in KeyValuePair<string, OpenApiPathItem> path, string transformValue)
    {
        var existsInStarting = path.Key.StartsWith(transformValue, StringComparison.Ordinal);
        if (!existsInStarting)
            return path;

        var newPathKey = path.Key.Substring(transformValue.Length);
        return new KeyValuePair<string, OpenApiPathItem>(newPathKey, path.Value);
    }
}
