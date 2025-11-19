using System.Globalization;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Frametux.Shared.Core.Driven.Validation;

public static class ServiceCollectionExt
{
    public static void AddFluentValidation(this IServiceCollection services, Assembly currentAssembly)
    {
        ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
        services.AddValidatorsFromAssembly(currentAssembly);
    }
}