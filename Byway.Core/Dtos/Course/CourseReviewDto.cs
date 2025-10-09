namespace Byway.Core.Dtos.Course
{
    public class CourseReviewDto
    {
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string? Message { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}