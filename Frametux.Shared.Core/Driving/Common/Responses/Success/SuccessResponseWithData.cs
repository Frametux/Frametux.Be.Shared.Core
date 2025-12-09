namespace Frametux.Shared.Core.Driving.Common.Responses.Success;

public record SuccessResponseWithData<TData> : SuccessResponse
{
    public required TData Data { get; init; }
}