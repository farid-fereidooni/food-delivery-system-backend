using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using RestaurantManagement.Write.Api.Filters;
using RestaurantManagement.Shared.Resources;

namespace RestaurantManagement.Write.Api.Pipelines;

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
            .AddJsonOptions(
                options =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(enumConverter);
                })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(CommonResource));
            });

        return builder;
    }
}
