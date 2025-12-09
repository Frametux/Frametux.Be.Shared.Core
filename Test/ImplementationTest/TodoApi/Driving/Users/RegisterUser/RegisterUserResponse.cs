using Frametux.Shared.Core.Driving.Common.Responses.Success;

namespace TodoApi.Driving.Users.RegisterUser;

public record RegisterUserResponse : SuccessResponseWithData<RegisterUserResponseData>;

public record RegisterUserResponseData
{
    public required string Id { get; init; }
}