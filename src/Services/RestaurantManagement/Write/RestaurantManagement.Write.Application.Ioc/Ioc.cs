using System.Windows.Input;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RestaurantManagement.Write.Application.Ioc;

public static class Ioc
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config
            .RegisterServicesFromAssembly(typeof(ICommand).Assembly));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EventPublishBehavior<,>));

        return builder;
    }
}
