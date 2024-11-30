using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Domain.Exceptions;

public class InvalidDomainOperationException : DomainException
{
    private InvalidDomainOperationException(string message) : base(message)
    {
    }

    public static void ThrowIfError<TError>(IResult<TError> result)
        where TError : IError
    {
        if (result.IsSuccess)
            return;

        var errorDescription = GetErrorMessages(result.UnwrapError());
        throw Create(errorDescription);
    }

    public static InvalidDomainOperationException Create(string errorDescription)
    {
        return new InvalidDomainOperationException(
            $"Invalid domain operation with following error(s): \n {errorDescription}");
    }
}
