using RestaurantManagement.Read.Application.Query;
using RestaurantManagement.Read.Application.Services;

namespace RestaurantManagement.Read.Api.Pipelines;

public static class ApplicationServicesPipeline
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config
            .RegisterServicesFromAssembly(typeof(IQuery<>).Assembly));

        builder.Services.AddScoped<IRestaurantOwnerService, RestaurantOwnerService>();

        return builder;
    }
}
