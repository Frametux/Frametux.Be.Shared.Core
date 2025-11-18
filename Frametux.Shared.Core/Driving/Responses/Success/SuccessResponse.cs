using System.Text.Json.Serialization;

namespace Frametux.Shared.Core.Driving.Responses.Success;

public record SuccessResponse() : BaseResponse(IsSuccess: true)
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SuccessType SuccessType { get; init; }
}

public enum SuccessType
{
    RetrieveDataSuccess = 0,
    CreateSuccess = 1
}