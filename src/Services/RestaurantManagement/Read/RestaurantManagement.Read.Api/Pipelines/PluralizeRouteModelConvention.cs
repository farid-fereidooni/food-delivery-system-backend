using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RestaurantManagement.Read.Domain.Helpers;

namespace RestaurantManagement.Read.Api.Pipelines;

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
