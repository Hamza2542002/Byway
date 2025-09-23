using Byway.Core.Entities.Enums;

namespace Byway.Core.Entities;

public class Course : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Cost { get; set; }
    public double TotalHours { get; set; }
    public double Rate { get; set; }
    public string? Certification { get; set; }
    public CourseLevel Level { get; set; }
    public Guid InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<CourseLecture>? Lectures { get; set; }
}
