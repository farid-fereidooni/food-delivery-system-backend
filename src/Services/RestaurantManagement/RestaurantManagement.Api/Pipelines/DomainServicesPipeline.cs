using MediatR;
using RestaurantManagement.Application;
using RestaurantManagement.Application.Command;
using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Api.Pipelines;

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
