using FluentValidation;

namespace TodoApi.Domain.UserAggregate.ValueObj;

public class Password
{
    public string Value { get; private init; }
    
    public const int MinLength = 6;
    public const int MaxLength = 1000;
    public static InlineValidator<string> Validator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MinimumLength(MinLength)
            .MaximumLength(MaxLength)
    };
    
    public Password(string value)
    {
        Validator.ValidateAndThrow(value);
        Value = value;
    }
    
    public static implicit operator string(Password password) => password.Value;
    public static implicit operator Password(string value) => new(value);
}
