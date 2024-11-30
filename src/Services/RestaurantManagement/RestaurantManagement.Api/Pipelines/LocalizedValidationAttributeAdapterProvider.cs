using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using RestaurantManagement.Api.Helpers;
using RestaurantManagement.Domain.Resources;

namespace RestaurantManagement.Api.Pipelines;

public class LocalizedValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _originalProvider = new();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        var errorMessage = attribute.ErrorMessage;
        if (errorMessage == null)
            return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);

        var translation = CommonResource.ResourceManager.GetString(errorMessage);
        if (translation == null)
            return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);

        attribute.ErrorMessage = translation.FormatError(errorMessage);
        return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
