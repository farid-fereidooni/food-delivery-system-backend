using System.Security.Claims;
using Identity.Core.Helpers;
using Identity.Core.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Identity.Api.Services;

public class OpenIdDictAuthorizationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly HttpContext _httpContext;
    private readonly ClientConfiguration _clientConfiguration;

    public OpenIdDictAuthorizationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictAuthorizationManager authorizationManager,
        IOpenIddictScopeManager scopeManager,
        IHttpContextAccessor httpContextAccessor,
        IOptions<ClientConfiguration> clientConfiguration
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationManager = applicationManager;
        _authorizationManager = authorizationManager;
        _scopeManager = scopeManager;
        _httpContext = httpContextAccessor.HttpContext!;
        _clientConfiguration = clientConfiguration.Value;
    }

    public async Task<Result<ClaimsIdentity, OpenIdAuthorizeError>> Authorize()
    {
        var request = _httpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var authenticationResult = await _httpContext.AuthenticateAsync();

        if(!IsUserLoggedIn(request, authenticationResult) || request.HasPrompt(OpenIddictConstants.Prompts.Login))
        {
            if (request.HasPrompt(OpenIddictConstants.Prompts.None))
                return new OpenIdAuthorizeError.LoginRequired();

            var prompt = string.Join(" ", request.GetPrompts().Remove(OpenIddictConstants.Prompts.Login));

            var parameters = _httpContext.Request.HasFormContentType
                ? _httpContext.Request.Form.Where(
                    parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList()
                : _httpContext.Request.Query.Where(
                    parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(OpenIddictConstants.Parameters.Prompt, new StringValues(prompt)));

            var redirectUrl = _httpContext.Request.PathBase
                + _httpContext.Request.Path
                + QueryString.Create(parameters);

            return new OpenIdAuthorizeError.DoLogin(redirectUrl);
        }

        var user = await _userManager.GetUserAsync(authenticationResult.Principal!) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        if (string.IsNullOrEmpty(request.ClientId))
            return new OpenIdAuthorizeError.InvalidClient("Client ID is required.");

        if (!_clientConfiguration.Clients.TryGetValue(request.ClientId, out var clientConfig))
            return new OpenIdAuthorizeError.DoLogin("Invalid client ID.");

        var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
        if (application is null)
            return new OpenIdAuthorizeError.DoLogin("Invalid client ID.");

        var applicationId = await _applicationManager.GetIdAsync(application);
        if (applicationId is null)
            throw new InvalidOperationException(
                $"Corresponding application ID not found for application {application}");

        var authorizations = await _authorizationManager.FindAsync(
            subject: await _userManager.GetUserIdAsync(user),
            client : applicationId,
            status : OpenIddictConstants.Statuses.Valid,
            type   : OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes : request.GetScopes()).ToListAsync();

        switch (await _applicationManager.GetConsentTypeAsync(application))
        {
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count is 0:
                return new OpenIdAuthorizeError.ConsentRequired(
                    "The logged in user is not allowed to access this client application.");

            case OpenIddictConstants.ConsentTypes.Implicit:
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count is not 0:
            case OpenIddictConstants.ConsentTypes.Explicit when authorizations.Count is not 0
                && !request.HasPrompt(OpenIddictConstants.Prompts.Consent):

                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: OpenIddictConstants.Claims.Name,
                    roleType: OpenIddictConstants.Claims.Role);

                identity.SetClaim(OpenIddictConstants.Claims.Subject, await _userManager.GetUserIdAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.Email, await _userManager.GetEmailAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.Name, await _userManager.GetUserNameAsync(user))
                    .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await _userManager.GetUserNameAsync(user))
                    .SetClaims(OpenIddictConstants.Claims.Role, [.. (await _userManager.GetRolesAsync(user))]);

                identity.SetScopes(request.GetScopes().Concat(clientConfig.AdditionalScopes));
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

                var authorization = authorizations.LastOrDefault();
                authorization ??= await _authorizationManager.CreateAsync(
                    identity: identity,
                    subject : await _userManager.GetUserIdAsync(user),
                    client  : applicationId,
                    type    : OpenIddictConstants.AuthorizationTypes.Permanent,
                    scopes  : identity.GetScopes());

                identity.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));
                identity.SetDestinations(GetDestinations);

                return identity;

            case OpenIddictConstants.ConsentTypes.Explicit when request.HasPrompt(OpenIddictConstants.Prompts.None):
            case OpenIddictConstants.ConsentTypes.Systematic when request.HasPrompt(OpenIddictConstants.Prompts.None):
                return new OpenIdAuthorizeError.ConsentRequired("Interactive user consent is required.");

            // In every other case, render the consent form.
            default: throw new NotImplementedException("Explicit consent is not supported");
        }
    }

    public async Task<Result<ClaimsIdentity, OpenIdExchangeError>> Exchange()
    {
        var request = _httpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            return new OpenIdExchangeError.InvalidGrant();

        var authenticationResult = await _httpContext.AuthenticateAsync(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var subjectId = authenticationResult.Principal?.GetClaim(OpenIddictConstants.Claims.Subject);
        if (authenticationResult.Principal is null || subjectId is null)
            return new OpenIdExchangeError.InvalidToken("The token is no longer valid.");

        var user = await _userManager.FindByIdAsync(subjectId);
        if (user is null)
            return new OpenIdExchangeError.InvalidToken("The token is no longer valid.");

        if (!await _signInManager.CanSignInAsync(user))
            return new OpenIdExchangeError.InvalidToken("The user is no longer allowed to sign in.");

        var identity = new ClaimsIdentity(authenticationResult.Principal.Claims,
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, await _userManager.GetUserIdAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Email, await _userManager.GetEmailAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Name, await _userManager.GetUserNameAsync(user))
            .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await _userManager.GetUserNameAsync(user))
            .SetClaims(OpenIddictConstants.Claims.Role, [.. (await _userManager.GetRolesAsync(user))]);

        identity.SetDestinations(GetDestinations);

        return identity;
    }


    private static bool IsUserLoggedIn(
        OpenIddictRequest openIdDictRequest, AuthenticateResult authenticationResult)
    {
        return authenticationResult.Succeeded
               && (openIdDictRequest.MaxAge == null
                   || authenticationResult.Properties?.IssuedUtc == null
                   || !(DateTimeOffset.UtcNow - authenticationResult.Properties.IssuedUtc >
                        TimeSpan.FromSeconds(openIdDictRequest.MaxAge.Value)));
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        switch (claim.Type)
        {
            case OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.PreferredUsername:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (claim.Subject?.HasScope(OpenIddictConstants.Scopes.Profile) ?? false)
                    yield return OpenIddictConstants.Destinations.IdentityToken;

                yield break;

            case OpenIddictConstants.Claims.Email:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (claim.Subject?.HasScope(OpenIddictConstants.Scopes.Email) ?? false)
                    yield return OpenIddictConstants.Destinations.IdentityToken;

                yield break;

            case OpenIddictConstants.Claims.Role:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (claim.Subject?.HasScope(OpenIddictConstants.Scopes.Roles) ?? false)
                    yield return OpenIddictConstants.Destinations.IdentityToken;

                yield break;

            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return OpenIddictConstants.Destinations.AccessToken;
                yield break;
        }
    }
}
