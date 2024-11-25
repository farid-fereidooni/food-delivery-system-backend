using RestaurantManagement.Core.Application.Services;
using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Api.Pipelines;

public static class CoreServicesPipeline
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConcurrencyHandler, ConcurrencyHandler>();

        return builder;
    }
}
