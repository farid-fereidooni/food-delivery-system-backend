using MediatR;
using RestaurantManagement.Application;
using RestaurantManagement.Application.Command;

namespace RestaurantManagement.Api.Pipelines;

public static class ApplicationServicesPipeline
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config
            .RegisterServicesFromAssembly(typeof(ICommand).Assembly));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EventPublishBehavior<,>));

        return builder;
    }
}
