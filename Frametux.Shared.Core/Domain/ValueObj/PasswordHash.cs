using FluentValidation;

namespace Frametux.Shared.Core.Domain.ValueObj;

public class PasswordHash
{
    public string Value { get; private init; }
    
    public const int MaxLength = 512;
    public static InlineValidator<string> Validator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MaximumLength(MaxLength)
    };
    
    public PasswordHash(string value)
    {
        Validator.ValidateAndThrow(value);
        Value = value;
    }
    
    public static implicit operator string(PasswordHash passwordHash) => passwordHash.Value;
    public static implicit operator PasswordHash(string value) => new(value);
}
