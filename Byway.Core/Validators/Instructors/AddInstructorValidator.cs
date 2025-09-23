using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities.Enums;
using FluentValidation;
using System.Reflection;
using System.Runtime.Serialization;

namespace Byway.Core.Validators.Instructors;

public class AddInstructorValidator : AbstractValidator<InstructorDto>
{
    int _rateMinValue = 0;
    int _rateMaxValue = 5;
    public AddInstructorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Full name is required.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Rate)
            .NotNull().WithMessage("Rate is required.")
            .LessThanOrEqualTo(_rateMaxValue).WithMessage($"Rate Must be less than or equal {_rateMaxValue}.")
            .GreaterThanOrEqualTo(_rateMinValue).WithMessage($"Rate Must be greater than or equal {_rateMinValue}.");
        RuleFor(x => x.JobTitle)
            .Must(value => Enum.GetValues(typeof(JobTitle))
                .Cast<JobTitle>()
                .Any(e => e.ToString() == value ||
                          e.GetType().GetMember(e.ToString())[0]
                            .GetCustomAttribute<EnumMemberAttribute>()?.Value == value))
            .WithMessage("Invalid JobTitle value.");
    }
}
