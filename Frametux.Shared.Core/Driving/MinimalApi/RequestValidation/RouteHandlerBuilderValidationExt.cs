using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Frametux.Shared.Core.Driving.MinimalApi.RequestValidation;

public static class RouteHandlerBuilderValidationExt
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<RequestValidationFilter<TRequest>>();
    }
}