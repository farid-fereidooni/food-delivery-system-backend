using System.Security.Claims;
using Identity.Api.Services;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AuthorizeResult =
    Identity.Core.Models.Result<System.Security.Claims.ClaimsIdentity, Identity.Core.Models.IOpenIdAuthorizeError>;
using ExchangeResult =
    Identity.Core.Models.Result<System.Security.Claims.ClaimsIdentity, Identity.Core.Models.IOpenIdExchangeError>;

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

        return authorizeResult switch
        {
            AuthorizeResult.Ok ok =>
                SignIn(new ClaimsPrincipal(ok.Value), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
            AuthorizeResult.Error err =>
                err.Value switch
                {
                    IOpenIdAuthorizeError.LoginRequired =>
                        Forbid(Errors.LoginRequired, "The user is not logged in."),
                    IOpenIdAuthorizeError.DoLogin doLogin =>
                        Challenge(new AuthenticationProperties
                        {
                            RedirectUri = doLogin.RedirectUrl
                        }),
                    IOpenIdAuthorizeError.InvalidClient invalidClient =>
                        Forbid(Errors.InvalidClient, invalidClient.Message),
                    IOpenIdAuthorizeError.ConsentRequired consentRequired =>
                        Forbid(Errors.ConsentRequired, consentRequired.Message),
                    _ => throw new ArgumentOutOfRangeException()
                },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var exchangeResult = await _authorizationService.Exchange();

        return exchangeResult switch
        {
            ExchangeResult.Ok ok =>
                SignIn(new ClaimsPrincipal(ok.Value), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
            ExchangeResult.Error err =>
                err.Value switch
                {
                    IOpenIdExchangeError.InvalidGrant => Forbid(Errors.InvalidGrant, string.Empty),
                    IOpenIdExchangeError.InvalidToken invalidToken => Forbid(Errors.InvalidGrant, invalidToken.Message),
                    IOpenIdExchangeError.InvalidUser invalidUser => Forbid(Errors.InvalidGrant, invalidUser.Message),
                    _ => throw new ArgumentOutOfRangeException()
                },
            _ => throw new ArgumentOutOfRangeException()
        };
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
