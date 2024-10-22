using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Models;
using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Api.Helpers;

public static class ApiHelpers
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
}
