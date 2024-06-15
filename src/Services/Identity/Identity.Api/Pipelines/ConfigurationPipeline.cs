using Identity.Core.Models;

namespace Identity.Api.Pipelines;

public static class ConfigurationPipeline
{
    public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ClientConfiguration>(builder.Configuration.GetSection(nameof(ClientConfiguration)));

        return builder;
    }

}
