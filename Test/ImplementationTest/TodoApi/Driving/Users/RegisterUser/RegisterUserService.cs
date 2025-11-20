using Frametux.Shared.Core.Driving.ApiTypes.MinimalApi;
using Frametux.Shared.Core.Driving.ApiTypes.MinimalApi.RequestValidation;
using Frametux.Shared.Core.Driving.Responses;
using Frametux.Shared.Core.Driving.Responses.Error;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Domain.UserAggregate.Entities;
using TodoApi.Domain.UserAggregate.Exceptions;
using TodoApi.Domain.UserAggregate.Services;
using TodoApi.Driven.DomainDb; 

namespace TodoApi.Driving.Users.RegisterUser;

public class RegisterUserService(IUserService userService, DomainDbContext dbContext) : IMinimalApi
{
    private async Task<BaseResponse> ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        User user;
        try
        {
            user = await userService.CreateUserAsync(
                request.Email,
                request.Password,
                cancellationToken);
        }
        catch (DuplicatedUserEmailExc exc)
        {
            return ErrorWithDetailsResponse
                .ToErrorResponse(nameof(request.Email), exc);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponse
        {
            Message = "Registered user successfully.",
            Type = ResponseType.CreateSuccess,
            Data = new RegisterUserResponseData
            {
                Id = user.Id
            }
        };
    }
    
    public static void RegisterRestfulApi(IEndpointRouteBuilder app) 
        => app
            .MapPost(EndPoints.RestApi.V1.Users, async (
                RegisterUserService registerUserService,
                [FromBody] RegisterUserRequest request,
                CancellationToken cancellationToken) =>
            {
                var response = await registerUserService.ExecuteAsync(request, cancellationToken);

                if (!response.IsSuccess) 
                    return Results.BadRequest(response);
                
                var userId = ((RegisterUserResponse)response).Data.Id;
                return Results.Created($"{EndPoints.RestApi.V1.Users}/{userId}", response);
            })
            .WithRequestValidation<RegisterUserRequest>(); 
}
