using Microsoft.AspNetCore.Routing;

namespace Frametux.Shared.Core.Driving.ApiTypes.MinimalApi;

public interface IMinimalApi
{
    static abstract void RegisterRestfulApi(IEndpointRouteBuilder app);
}