namespace Identity.Core.Models;

public interface IOpenIdExchangeError : IRichEnum
{
    public record struct InvalidGrant : IOpenIdExchangeError;
    public record struct InvalidToken(string Message) : IOpenIdExchangeError;
    public record struct InvalidUser(string Message) : IOpenIdExchangeError;
}
