namespace Byway.Core.Entities;

public class CourseLecture : BaseEntity
{
    public string? Name { get; set; }
    public int Number { get; set; }
    public double Time { get; set; }
    public Guid CourseId { get; set; }
}