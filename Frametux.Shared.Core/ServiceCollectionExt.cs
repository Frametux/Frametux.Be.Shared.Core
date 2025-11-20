using System.Reflection;
using Frametux.Shared.Core.Driven.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Frametux.Shared.Core;

public static class ServiceCollectionExt
{
    public static void AddSharedCore(this IServiceCollection services, Assembly currentAssembly)
    {
        services.AddFluentValidation(currentAssembly);
    }
}