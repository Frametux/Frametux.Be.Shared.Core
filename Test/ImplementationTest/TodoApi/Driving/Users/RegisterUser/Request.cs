using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObjs;
using TodoApi.Domain.UserAggregate.ValueObjs;

namespace TodoApi.Driving.Users.RegisterUser;

public record RegisterUserRequest(string Email, string Password);

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(r => r.Email)
            .SetValidator(Email.Validator);
            
        RuleFor(r => r.Password)
            .SetValidator(Password.Validator);
    }
}