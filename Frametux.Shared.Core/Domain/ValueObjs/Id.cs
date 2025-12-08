using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObjs.Base;

namespace Frametux.Shared.Core.Domain.ValueObjs;

public record Id : ISinglePropValueObj<string>
{
    public string Value { get; }
    
    public const int MaxLength = 255;
    public static InlineValidator<string> Validator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MaximumLength(MaxLength)
    };
    
    public Id(string value)
    {
        Validator.ValidateAndThrow(value);
        Value = value;
    }

    public Id()
    {
        Value = Guid.NewGuid().ToString();
    }
    
    public static implicit operator string(Id id) => id.Value;
    public static implicit operator Id(string value) => new(value);
}