namespace Byway.Core.Dtos.Course;

public class CourseToReturnDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Cost { get; set; }
    public double TotalHours { get; set; }
    public double Rate { get; set; }
    public string? Certification { get; set; }
    public string? Level { get; set; }
    public CategoryDto? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<CourseLectureToReturnDto>? Lectures { get; set; }
    public List<CourseListToReturnDto> RelatedCourses { get; set; } = [];
    public CourseInstructorDto? Instructor { get; set; }
    public List<CourseReviewDto> Reviews { get; set; } = [];
}
