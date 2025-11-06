using FluentValidation;

namespace Frametux.Shared.Core.Domain.ValueObj;

public record Email
{
    public string Value { get; private init; }
    
    public const int MaxLength = 320;
    public static InlineValidator<string> Validator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MaximumLength(MaxLength)
            .EmailAddress()
    };
    
    public Email(string value)
    {
        var normalizedValue = value.ToLowerInvariant();
        Validator.ValidateAndThrow(normalizedValue);
        Value = normalizedValue;
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);
}
