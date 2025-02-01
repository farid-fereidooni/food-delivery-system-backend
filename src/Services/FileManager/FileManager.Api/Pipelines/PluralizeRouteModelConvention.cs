using FileManager.Core.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Humanizer;

namespace FileManager.Api.Pipelines;

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
