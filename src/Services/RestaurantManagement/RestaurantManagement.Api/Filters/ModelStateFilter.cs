using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantManagement.Api.Helpers;

namespace RestaurantManagement.Api.Filters;

public class ModelStateFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
            await next();

        context.Result = context.ModelState.ToErrorResult();
    }
}
