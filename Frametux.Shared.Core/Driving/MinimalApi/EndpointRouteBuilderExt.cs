using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Frametux.Shared.Core.Driving.MinimalApi;

public static class EndpointRouteBuilderExt
{
    public static void UseMinimalApis(this IEndpointRouteBuilder app, Assembly currentAssembly)
    {
        var apiTypes = currentAssembly.GetTypes()
            .Where(t => 
                !t.IsAbstract && 
                !t.IsInterface && 
                t.IsPublic && 
                typeof(IMinimalApi).IsAssignableFrom(t)
            );
        
        foreach (var apiType in apiTypes)
        {
            var registerMethod = apiType.GetMethod(
                nameof(IMinimalApi.RegisterRestfulApi), 
                BindingFlags.Public | BindingFlags.Static
            );

            if (registerMethod is not null)
                registerMethod.Invoke(null, [app]);
            else
                throw new InvalidOperationException(
                    $"Type {apiType.Name} must implement {nameof(IMinimalApi)}");
        }
    }
}