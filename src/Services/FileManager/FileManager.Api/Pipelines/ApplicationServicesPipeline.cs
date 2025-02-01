namespace FileManager.Api.Pipelines;

public static class ApplicationServicesPipeline
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<Program>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return builder;
    }
}
