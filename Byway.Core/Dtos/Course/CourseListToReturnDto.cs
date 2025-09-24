using Byway.Core.Entities.Enums;

namespace Byway.Core.Dtos.Course;

public class CourseListToReturnDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Cost { get; set; }
    public double TotalHours { get; set; }
    public double Rate { get; set; }
    public string? Certification { get; set; }
    public CourseLevel Level { get; set; }
    public Guid InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int LecturesCount { get; set; }
}
