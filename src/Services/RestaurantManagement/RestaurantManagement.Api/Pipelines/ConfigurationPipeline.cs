using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Api.Pipelines;

public static class ConfigurationPipeline
{
    public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IdentityConfiguration>(builder.Configuration.GetSection(nameof(IdentityConfiguration)));

        return builder;
    }
}
