using System.Text;
using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {

    }

    protected static string GetErrorMessages(IError error)
    {
        var stringBuilder = new StringBuilder();

        foreach (var message in error.Messages)
        {
            stringBuilder.AppendLine(message.Description);
        }

        return stringBuilder.ToString();
    }
}
