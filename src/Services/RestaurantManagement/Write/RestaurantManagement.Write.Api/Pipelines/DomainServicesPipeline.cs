using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class DomainServicesPipeline
{
    public static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<Entity>()
                .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime());

        return builder;
    }
}
