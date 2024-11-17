using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Core.Domain.Exceptions;

public class InvalidDomainStateException : DomainException
{
    private InvalidDomainStateException(string message) : base(message)
    {
    }

    public static void ThrowIfError<TError>(IResult<TError> result) where TError : IError
    {
        if (result.IsSuccess)
            return;

        var errorDescription = GetErrorMessages(result.UnwrapError());
        throw Create(errorDescription);
    }

    public static InvalidDomainStateException Create(string errorDescription)
    {
        throw new InvalidDomainStateException(
            $"Domain model entered invalid state with following error(s): \n {errorDescription}");
    }
}
