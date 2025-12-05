using FluentValidation;

namespace Frametux.Shared.Core.Domain.ValueObjs;

public record CreatedAt : UtcDateTime
{
    public new static InlineValidator<DateTime> Validator { get; } = new()
    {
        v => v.RuleFor(dt => dt)
            .SetValidator(UtcDateTime.Validator)
            .LessThanOrEqualTo(_ => DateTime.UtcNow)
            .WithMessage("cannot be in the future.")
    };
    
    public CreatedAt(DateTime value) : base(value, false)
    {
        Validator.ValidateAndThrow(Value);
    }

    public CreatedAt()
    {
    }
    
    public static implicit operator CreatedAt(DateTime value) => new(value);
}
