using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using RestaurantManagement.Read.Api.Helpers;
using RestaurantManagement.Read.Application.Services;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Dtos;

namespace RestaurantManagement.Read.Api.Pipelines;

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

        builder.Services.AddSingleton<IAuthorizationHandler, RestaurantOwnerRequirementHandler>();
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(
                Constants.RestaurantOwnerPolicy,
                config => config.AddRequirements(new RestaurantOwnerRequirement()));

        return builder;
    }
}

public class RestaurantOwnerRequirement : IAuthorizationRequirement;

public class RestaurantOwnerRequirementHandler : AuthorizationHandler<RestaurantOwnerRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public RestaurantOwnerRequirementHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RestaurantOwnerRequirement requirement)
    {
        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var service = scope.ServiceProvider.GetRequiredService<IRestaurantOwnerService>();

        var result = await authService
            .GetCurrentUserId()
            .Match(
                id => service.IsRestaurantOwner(id),
                error => Task.FromResult(false));

        if (result)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
