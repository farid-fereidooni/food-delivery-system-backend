using RestaurantManagement.Application.Services;
using RestaurantManagement.Domain.Contracts;

namespace RestaurantManagement.Api.Pipelines;

public static class CoreServicesPipeline
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConcurrencyHandler, ConcurrencyHandler>();

        return builder;
    }
}
