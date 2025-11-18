namespace Frametux.Shared.Core.Driving.Responses.Success;

public record SuccessResponseWithData<TData> : SuccessResponse
{
    public required TData Data { get; init; }
}