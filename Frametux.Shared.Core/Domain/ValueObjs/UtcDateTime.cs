using FluentValidation;

namespace Frametux.Shared.Core.Domain.ValueObjs;

public record UtcDateTime
{
    public DateTime Value { get; protected init; }
    
    public static InlineValidator<DateTime> Validator { get; } = new()
    {
        v => v.RuleFor(dt => dt)
            .Must(dt => dt.Kind == DateTimeKind.Utc)
            .WithMessage("must be UTC.")
    };
    
    protected UtcDateTime(DateTime value, bool shouldValidate = true)
    {
        var utcValue = ConvertToUtc(value);

        if (shouldValidate)
            Validator.ValidateAndThrow(utcValue);
        
        Value = utcValue;
    }
    
    public UtcDateTime(DateTime value)
    {
        var utcValue = ConvertToUtc(value);

        Validator.ValidateAndThrow(utcValue);
        
        Value = utcValue;
    }

    public static DateTime ConvertToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => throw new ArgumentException("Invalid DateTimeKind") // This is impossible (also in unittest), but just in case
        };
    }

    public UtcDateTime()
    {
        Value = DateTime.UtcNow;
    }
    
    public static implicit operator DateTime(UtcDateTime utcDateTime) => utcDateTime.Value;
    public static implicit operator UtcDateTime(DateTime value) => new(value);
}

