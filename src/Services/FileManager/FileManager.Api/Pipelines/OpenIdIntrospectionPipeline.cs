using FileManager.Core.Models;
using OpenIddict.Validation.AspNetCore;

namespace FileManager.Api.Pipelines;

public static class OpenIdIntrospectionPipeline
{
    public static WebApplicationBuilder AddCustomOpenIdServer(this WebApplicationBuilder builder)
    {
        var identityConfiguration = builder.Configuration.GetSection(nameof(IdentityConfiguration));
        builder.Services.Configure<IdentityConfiguration>(identityConfiguration);

        builder.Services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(identityConfiguration[nameof(IdentityConfiguration.Issuer)]!);
                options.AddAudiences(identityConfiguration[nameof(IdentityConfiguration.Audience)]!);

                options.UseIntrospection()
                    .SetClientId(identityConfiguration[nameof(IdentityConfiguration.ClientId)]!)
                    .SetClientSecret(identityConfiguration[nameof(IdentityConfiguration.ClientSecret)]!);

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        return builder;
    }
}
