using FileManager.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileManager.Api.Filters;

public class ModelStateFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
            await next();

        context.Result = context.ModelState.ToErrorResult();
    }
}
