namespace Frametux.Shared.Core.Driving.Responses;

// ReSharper disable once NotAccessedPositionalProperty.Global
public abstract record BaseResponse(bool IsSuccess)
{
    public required string Message { get; init; }
}