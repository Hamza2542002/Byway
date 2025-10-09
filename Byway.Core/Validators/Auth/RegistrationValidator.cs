using Byway.Core.Auth;
using FluentValidation;

namespace Byway.Core.Validators.Auth;

public class RegistrationValidator : AbstractValidator<RegistrationDTO>
{
    public RegistrationValidator()
    {
        RuleFor(e => e.Email)
            .NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email Format is not right");

        RuleFor(e => e.Password)
            .NotNull().WithMessage("Password is Required");
        RuleFor(e => e.ConfirmPassword)
            .NotNull().WithMessage("Confirm Password is Required")
            .Matches(e => e.Password).WithMessage("Password and Confirm Password don't match");
        RuleFor(e => e.Username)
            .NotNull().WithMessage("Username is Required");
        RuleFor(e => e.FirstName)
            .NotNull().WithMessage("First Name is Required");
        RuleFor(e => e.LastName)
            .NotNull().WithMessage("Last Name is Required");

    }
}
