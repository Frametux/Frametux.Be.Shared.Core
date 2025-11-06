using FluentValidation;

namespace Frametux.Shared.Core.Domain.ValueObj;

public record CreatedAt
{
    public DateTime Value { get; }
    
    public static InlineValidator<DateTime> Validator { get; } = new()
    {
        v => v.RuleFor(dt => dt)
            .LessThanOrEqualTo(_ => DateTime.UtcNow)
            .WithMessage("cannot be in the future")
            .Must(dt => dt.Kind == DateTimeKind.Utc)
            .WithMessage("must be UTC")
    };
    
    public CreatedAt(DateTime value)
    {
        var utcValue = value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => throw new ArgumentException("Invalid DateTimeKind")
        };
        
        Validator.ValidateAndThrow(utcValue);
        Value = utcValue;
    }

    public CreatedAt()
    {
        Value = DateTime.UtcNow;
    }
    
    public static implicit operator DateTime(CreatedAt createdAt) => createdAt.Value;
    public static implicit operator CreatedAt(DateTime value) => new(value);
}
