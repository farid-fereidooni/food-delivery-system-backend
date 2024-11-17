using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Infrastructure.Database;
using RestaurantManagement.Infrastructure.Repositories.Command;

namespace RestaurantManagement.Api.Pipelines;

public static class InfrastructureServicesPipeline
{
    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("RelationalDatabase"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IUnitOfWork>(f => f.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<IRestaurantOwnerCommandRepository, RestaurantOwnerCommandRepository>();
        builder.Services.AddScoped<IMenuCommandRepository, MenuCommandRepository>();

        return builder;
    }
}
