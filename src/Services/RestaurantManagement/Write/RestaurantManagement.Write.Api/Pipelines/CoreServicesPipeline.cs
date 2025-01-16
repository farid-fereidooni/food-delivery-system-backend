using RestaurantManagement.Write.Application.Services;
using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class CoreServicesPipeline
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConcurrencyHandler, ConcurrencyHandler>();

        return builder;
    }
}
