using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestaurantManagement.Api.Dtos;
using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Api.Helpers;

public static partial class ApiHelpers
{
    public static IActionResult ToApiResponse(this Result result)
    {
        return result.Match(
            () => new OkObjectResult(new GenericHttpResponse()),
            error => CreateErrorResponse(error));
    }

    public static IActionResult ToApiResponse<TData, TError>(this IResult<TData, TError> result)
        where TError : IError
    {
        return result.Match(
            data => new OkObjectResult(new GenericHttpResponse<TData>(data)),
            error => CreateErrorResponse(error));
    }

    private static IActionResult CreateErrorResponse(IError error)
    {
        return error.Reason switch
        {
            ErrorReason.General => new BadRequestObjectResult(new GenericHttpResponse(error)),
            ErrorReason.NotFound => new NotFoundObjectResult(new GenericHttpResponse(error)),
            ErrorReason.NotAuthenticated => new UnauthorizedObjectResult(new GenericHttpResponse(error)),
            ErrorReason.NotAllowed => new ObjectResult(new GenericHttpResponse(error))
            {
                StatusCode = StatusCodes.Status403Forbidden
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string FormatError(this string error, [CallerArgumentExpression("error")] string? code = null)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentException("Code not specified for error");

        return $"[{code}] {error}";
    }

    [GeneratedRegex(@"(\[.+\])\s?(.+)")]
    private static partial Regex ErrorFormatRegex();

    public static IActionResult ToErrorResult(this ModelStateDictionary modelStateDictionary)
    {
        var messages = modelStateDictionary.Values.SelectMany(s =>
            s.Errors.Select(err =>
            {
                var errorFormatMatch = ErrorFormatRegex().Match(err.ErrorMessage);
                if (errorFormatMatch is { Success: true, Groups.Count: 2 })
                    return new Message(errorFormatMatch.Groups[1].Value, errorFormatMatch.Groups[0].Value);

                return new Message(err.ErrorMessage, "ValidationError");
            })
        );

        return CreateErrorResponse(new Error(messages));
    }
}
