using System.Security.Cryptography.X509Certificates;
using Identity.Core.Models;
using Identity.Infrastructure.Database;

namespace Identity.Api.Pipelines;

public static class OpenIdServerPipeline
{
    public static WebApplicationBuilder AddCustomOpenIdServer(this WebApplicationBuilder builder)
    {
        var identityConfiguration = builder.Configuration.GetSection(nameof(IdentityConfiguration));
        builder.Services.Configure<IdentityConfiguration>(identityConfiguration);

        builder.Services.AddOpenIddict()
            .AddCore(options => options
                .UseEntityFrameworkCore()
                .UseDbContext<ApplicationDbContext>())
            .AddServer(options =>
            {
                options
                    .SetTokenEndpointUris("connect/token")
                    .SetAuthorizationEndpointUris("connect/authorize")
                    .AllowClientCredentialsFlow()
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                if (builder.Environment.IsDevelopment())
                {
                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate()
                        .SetAccessTokenLifetime(TimeSpan.FromDays(1))
                        .SetIdentityTokenLifetime(TimeSpan.FromDays(1));
                }
                else
                {
                    options
                        .AddEncryptionCertificate(new X509Certificate2(
                            identityConfiguration[nameof(IdentityConfiguration.EncryptionCertificatePath)]!))
                        .AddSigningCertificate(new X509Certificate2(
                            identityConfiguration[nameof(IdentityConfiguration.SigningCertificatePath)]!));
                }
            });

        return builder;
    }
}
