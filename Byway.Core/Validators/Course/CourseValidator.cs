using Byway.Core.Dtos.Course;
using Byway.Core.Entities.Enums;
using FluentValidation;
using System.Reflection;
using System.Runtime.Serialization;

namespace Byway.Core.Validators.Course;

public class CourseValidator : AbstractValidator<CourseDto>
{
    public CourseValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Name is required.");
        RuleFor(e => e.Cost)
            .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0.");
        RuleFor(e => e.TotalHours)
            .GreaterThan(0).WithMessage("TotalHours must be greater than 0.");
        RuleFor(e => e.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5.");
        RuleFor(e => e.Level)
            .NotEmpty().WithMessage("Level is required.");
        RuleFor(e => e.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
        RuleFor(e => e.InstructorId)
            .NotEmpty().WithMessage("InstructorId is required.");
        //RuleFor(e => e.Lectures)
        //    .NotEmpty().WithMessage("At least one lecture is required.");

        //RuleFor(e => e)
        //    .Must(e => e.Lectures is not null && e.Lectures.Sum(lec => lec.Time) == e.TotalHours)
        //    .WithMessage("Lectures Duration Must Equal Course Duration");
        RuleFor(x => x.Level)
            .Must(value => Enum.GetValues(typeof(CourseLevel))
                .Cast<CourseLevel>()
                .Any(e => e.ToString() == value ||
                          e.GetType().GetMember(e.ToString())[0]
                            .GetCustomAttribute<EnumMemberAttribute>()?.Value == value))
            .WithMessage("Invalid Course Level value.");
    }
}
