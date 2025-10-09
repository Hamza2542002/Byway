using Microsoft.AspNetCore.Identity;

namespace Byway.Core.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public List<CourseEnrollment>? Enrollments { get; set; }
}
