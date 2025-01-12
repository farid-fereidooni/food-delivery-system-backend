using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainServices;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Api.Pipelines;

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
    private readonly IRestaurantOwnerService _service;
    private readonly IAuthService _authService;

    public RestaurantOwnerRequirementHandler(IRestaurantOwnerService service, IAuthService authService)
    {
        _service = service;
        _authService = authService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RestaurantOwnerRequirement requirement)
    {
        var result = await _authService
            .GetCurrentUserId()
            .Match(
                id => _service.IsRestaurantOwner(id),
                error => Task.FromResult(false));

        if (result)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
