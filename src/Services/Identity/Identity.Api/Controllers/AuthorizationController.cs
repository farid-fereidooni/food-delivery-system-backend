using System.Security.Claims;
using Identity.Api.Services;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Controllers;

public class AuthorizationController : Controller
{
    private readonly OpenIdDictAuthorizationService _authorizationService;

    public AuthorizationController(OpenIdDictAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpGet("~/connect/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var authorizeResult = await _authorizationService.Authorize();

        return authorizeResult.Match(
            identity => SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
            error => error switch
            {
                OpenIdAuthorizeError.LoginRequired =>
                    Forbid(Errors.LoginRequired, "The user is not logged in."),
                OpenIdAuthorizeError.DoLogin doLogin =>
                    Challenge(new AuthenticationProperties
                    {
                        RedirectUri = doLogin.RedirectUrl
                    }),
                OpenIdAuthorizeError.InvalidClient invalidClient =>
                    Forbid(Errors.InvalidClient, invalidClient.Message),
                OpenIdAuthorizeError.ConsentRequired consentRequired =>
                    Forbid(Errors.ConsentRequired, consentRequired.Message),
                _ => throw new ArgumentOutOfRangeException()
            });
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var exchangeResult = await _authorizationService.Exchange();

        return exchangeResult.Match(
            identity => SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
            error => error switch
            {
                OpenIdExchangeError.InvalidGrant => Forbid(Errors.InvalidGrant, string.Empty),
                OpenIdExchangeError.InvalidToken invalidToken => Forbid(Errors.InvalidGrant, invalidToken.Message),
                OpenIdExchangeError.InvalidUser invalidUser => Forbid(Errors.InvalidGrant, invalidUser.Message),
                _ => throw new ArgumentOutOfRangeException()
            });
    }

    private IActionResult Forbid(string error, string errorDescription)
    {
        return Forbid(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
            }));
    }
}
