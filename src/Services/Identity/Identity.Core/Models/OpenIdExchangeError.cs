namespace Identity.Core.Models;

public record OpenIdExchangeError : IRichEnum, IError
{
    public OpenIdExchangeError(string code, string message, ErrorReason reason = ErrorReason.General)
    {
        Messages = [new Message(code, message)];
        Reason = reason;
    }

    public IReadOnlyCollection<Message> Messages { get; }
    public ErrorReason Reason { get; }
    public record InvalidGrant() : OpenIdExchangeError(nameof(InvalidGrant), "Invalid grant");
    public record InvalidToken(string Message) : OpenIdExchangeError(nameof(InvalidToken), Message);
    public record InvalidUser(string Message) : OpenIdExchangeError(nameof(InvalidUser), Message);
}
