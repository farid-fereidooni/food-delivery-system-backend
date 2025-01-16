using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RestaurantManagement.Write.Domain.Helpers;

namespace RestaurantManagement.Write.Api.Pipelines;

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
