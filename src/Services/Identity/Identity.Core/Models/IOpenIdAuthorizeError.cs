namespace Identity.Core.Models;

public interface IOpenIdAuthorizeError : IRichEnum
{
    public record struct LoginRequired : IOpenIdAuthorizeError;
    public record struct DoLogin(string RedirectUrl) : IOpenIdAuthorizeError;
    public record struct InvalidClient(string Message) : IOpenIdAuthorizeError;
    public record struct ConsentRequired(string Message) : IOpenIdAuthorizeError;
    public record struct DoConsent : IOpenIdAuthorizeError;
}
