using FluentValidation.Results;
using Frametux.Shared.Core.Domain.Exceptions;

namespace Frametux.Shared.Core.Driving.Responses.Error;

public record ErrorWithDetailsResponse : ErrorResponse
{
    public required List<Error> Errors { get; init; }
    
    public static ErrorWithDetailsResponse ToErrorResponse(ValidationResult validationResult)
    {
        return new ErrorWithDetailsResponse
        {
            Message = "Validation Failed.",
            ErrorType = ErrorType.ValidationFailed,
            Errors = validationResult.Errors
                .GroupBy(failure => failure.PropertyName)
                .Select(group =>
                {
                    return new Error
                    {
                        Field = group.Key,
                        Errors = group.Select(error => new ErrorContent
                        {
                            Code = error.ErrorCode,
                            Message = error.ErrorMessage,
                        }).ToList()
                    };
                }).ToList()
        };
    }

    public static ErrorWithDetailsResponse ToErrorResponse<TExc>(string fieldName, ErrorType errorType, TExc exc) where TExc : IExcHasErrorCode
    {
        return new ErrorWithDetailsResponse
        {
            Errors =
            [
                new Error
                {
                    Field = fieldName,
                    Errors =
                    [
                        new ErrorContent
                        {
                            Code = exc.Code,
                            Message = exc.Message
                        }
                    ]
                }
            ],
            Message = exc.Message,
            ErrorType = errorType
        };
    }
}

public record Error
{
    public required string Field { get; init; }
    public required List<ErrorContent> Errors { get; init; }
}

public record ErrorContent
{
    public required string Code { get; init; }
    public required string Message { get; init; }
}