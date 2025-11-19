using System.Text.Json.Serialization;

namespace Frametux.Shared.Core.Driving.Responses;

public abstract record BaseResponse(bool IsSuccess)
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ResponseType Type { get; init; }
    public required string Message { get; init; }
}

public enum ResponseType
{
    // Success
    RetrieveDataSuccess,
    CreateSuccess,
    
    // Error
    NotFound,
    ValidationFailed,
    BusinessLogicFailed
}