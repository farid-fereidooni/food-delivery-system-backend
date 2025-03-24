using MediatR;
using RestaurantManagement.Write.Application;
using RestaurantManagement.Write.Application.Command;

namespace RestaurantManagement.Write.Api.Pipelines;

public static class ApplicationServicesPipeline
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config
            .RegisterServicesFromAssembly(typeof(ICommand).Assembly));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return builder;
    }
}
