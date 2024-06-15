using System.ComponentModel;
using Identity.Core.Models;
using Microsoft.AspNetCore.Mvc;
using EmptyResult = Identity.Core.Models.EmptyResult;

namespace Identity.Api.Utilities;

public static class ResultExtensions
{
    public static IActionResult ToApiResult(this EmptyResult result)
    {
        return result switch
        {
            EmptyResult.Ok okResult => new OkObjectResult(new EmptyApiResult
            {
                Messages = okResult.Messages,
                IsSuccessful = true,
            }),
            EmptyResult.Error => new BadRequestObjectResult(new EmptyApiResult
            {
                Messages = result.Messages,
                IsSuccessful = false,
            }),
            _ => throw new InvalidEnumArgumentException(),
        };
    }

    public static IActionResult ToApiResult<TError>(this EmptyResult<TError> result)
    {
        return result switch
        {
            EmptyResult<TError>.Ok okResult => new OkObjectResult(new EmptyApiResult
            {
                Messages = okResult.Messages,
                IsSuccessful = true,
            }),
            EmptyResult<TError>.Error => new BadRequestObjectResult(new EmptyApiResult
            {
                Messages = result.Messages,
                IsSuccessful = false,
            }),
            _ => throw new InvalidEnumArgumentException(),
        };
    }

    public static IActionResult ToApiResult<TOk>(this Result<TOk> result)
    {
        return result switch
        {
            Result<TOk>.Ok okResult => new OkObjectResult(new ApiResult<TOk>()
            {
                Data = okResult.Value,
                Messages = okResult.Messages,
                IsSuccessful = true,
            }),
            Result<TOk>.Error => new BadRequestObjectResult(new ApiResult<TOk>()
            {
                Data = default,
                Messages = result.Messages,
                IsSuccessful = false,
            }),
            _ => throw new InvalidEnumArgumentException(),
        };
    }

    public static IActionResult ToApiResult<TOk, TError>(this Result<TOk, TError> result)
        where TError : IRichEnum
    {
        return result switch
        {
            Result<TOk, TError>.Ok okResult => new OkObjectResult(new ApiResult<TOk>()
            {
                Data = okResult.Value,
                Messages = okResult.Messages,
                IsSuccessful = true,
            }),
            Result<TOk, TError>.Error => new BadRequestObjectResult(new ApiResult<TOk>()
            {
                Data = default,
                Messages = result.Messages,
                IsSuccessful = false,
            }),
            _ => throw new InvalidEnumArgumentException(),
        };
    }
}
