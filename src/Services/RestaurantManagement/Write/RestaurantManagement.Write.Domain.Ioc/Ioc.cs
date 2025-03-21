using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Domain.Ioc;

public static class Ioc
{
    public static IHostApplicationBuilder AddDomainServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<Entity>()
            .AddClasses(classes => classes.Where(w => w.Name.EndsWith("Service")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return builder;
    }
}
