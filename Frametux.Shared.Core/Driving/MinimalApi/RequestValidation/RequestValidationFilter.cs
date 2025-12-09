using FluentValidation;
using Frametux.Shared.Core.Driving.Common.Responses.Error;
using Microsoft.AspNetCore.Http;

namespace Frametux.Shared.Core.Driving.MinimalApi.RequestValidation;

public class RequestValidationFilter<TRequest>(IValidator<TRequest>? validator = null) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (validator is null)
            return await next(context);

        var request = context.Arguments.OfType<TRequest>().First();
        
        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
            return Results.BadRequest(ErrorWithDetailsResponse.ToErrorResponse(validationResult));

        return await next(context);
    }
}