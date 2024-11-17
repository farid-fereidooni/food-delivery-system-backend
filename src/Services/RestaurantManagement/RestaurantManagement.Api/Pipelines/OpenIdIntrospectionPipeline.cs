using OpenIddict.Validation.AspNetCore;
using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Api.Pipelines;

public static class OpenIdIntrospectionPipeline
{
    public static WebApplicationBuilder AddCustomOpenIdServer(this WebApplicationBuilder builder)
    {
        var identityConfiguration = builder.Configuration.GetSection(nameof(IdentityConfiguration));
        builder.Services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(identityConfiguration[nameof(IdentityConfiguration.Issuer)]!);
                options.AddAudiences(identityConfiguration[nameof(IdentityConfiguration.Audience)]!);

                // Configure the validation handler to use introspection and register the client
                // credentials used when communicating with the remote introspection endpoint.
                options.UseIntrospection()
                    .SetClientId(identityConfiguration[nameof(IdentityConfiguration.ClientId)]!)
                    .SetClientSecret(identityConfiguration[nameof(IdentityConfiguration.ClientSecret)]!);

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();
        return builder;
    }
}
