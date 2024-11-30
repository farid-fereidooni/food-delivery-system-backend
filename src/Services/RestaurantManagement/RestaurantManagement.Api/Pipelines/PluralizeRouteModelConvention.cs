using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RestaurantManagement.Domain.Helpers;

namespace RestaurantManagement.Api.Pipelines;

public class PluralizeRouteModelConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var applicationController in application.Controllers)
        {
            applicationController.ControllerName = applicationController.ControllerName
                .PascalToKebabCase()
                .Pluralize();
        }
    }
}
