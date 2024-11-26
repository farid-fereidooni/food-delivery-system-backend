using Microsoft.AspNetCore.Mvc.DataAnnotations;
using RestaurantManagement.Api.Filters;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Pipelines;

public static class ControllerPipeline
{
    public static WebApplicationBuilder AddCustomControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<
            IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();

        builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ModelStateFilter>();
                options.Conventions.Add(new PluralizeRouteModelConvention());
            })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(CommonResource));
            });

        return builder;
    }
}
