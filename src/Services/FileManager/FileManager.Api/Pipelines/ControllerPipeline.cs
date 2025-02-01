using System.Text.Json.Serialization;
using FileManager.Api.Filters;
using FileManager.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace FileManager.Api.Pipelines;

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

        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        return builder;
    }
}
