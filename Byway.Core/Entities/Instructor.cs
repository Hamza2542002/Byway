using Byway.Core.Entities.Enums;

namespace Byway.Core.Entities;

public class Instructor : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double Rate { get; set; }
    public JobTitle JobTitle { get; set; }
    public List<Course>? Courses { get; set; }
}
