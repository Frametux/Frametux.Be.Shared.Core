using System.Text.Json.Serialization;

namespace Frametux.Shared.Core.Driving.Responses.Error;

public record ErrorResponse() : BaseResponse(IsSuccess: false)
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ErrorType ErrorType { get; init; }
}

public enum ErrorType
{
    NotFound = 0,
    ValidationFailed = 1,
    BusinessLogicFailed = 2,
}
