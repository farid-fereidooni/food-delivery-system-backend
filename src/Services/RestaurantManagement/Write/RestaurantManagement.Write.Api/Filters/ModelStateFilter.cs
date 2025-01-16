using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantManagement.Write.Api.Helpers;

namespace RestaurantManagement.Write.Api.Filters;

public class ModelStateFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
            await next();

        context.Result = context.ModelState.ToErrorResult();
    }
}
