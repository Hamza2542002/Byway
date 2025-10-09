using Byway.Core.Auth;
using FluentValidation;

namespace Byway.Core.Validators.Auth;

public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(e => e.Email).NotNull().WithMessage("Email is Required");
        RuleFor(e => e.Password)
            .NotNull().WithMessage("Password is Required");
    }
}
