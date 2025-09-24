namespace Byway.Core.Entities;

public class CourseEnrollment : BaseEntity
{
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
