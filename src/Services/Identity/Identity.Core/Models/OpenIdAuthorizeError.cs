namespace Identity.Core.Models;

public record OpenIdAuthorizeError : IRichEnum, IError
{
    public OpenIdAuthorizeError(string code, string message, ErrorReason reason = ErrorReason.General)
    {
        Messages = [new Message(code, message)];
        Reason = reason;
    }

    public IReadOnlyCollection<Message> Messages { get; }
    public ErrorReason Reason { get; }

    public record LoginRequired() : OpenIdAuthorizeError(
        nameof(LoginRequired), "Login is required", ErrorReason.NotAuthenticated);

    public record DoLogin(string RedirectUrl) : OpenIdAuthorizeError(
        nameof(DoLogin), "Login is required", ErrorReason.NotAuthenticated);

    public record InvalidClient(string Message) : OpenIdAuthorizeError(nameof(InvalidClient), Message);

    public record ConsentRequired(string Message) : OpenIdAuthorizeError(nameof(ConsentRequired), Message);

    public record DoConsent() : OpenIdAuthorizeError(nameof(DoConsent), "Consent is required");
}
