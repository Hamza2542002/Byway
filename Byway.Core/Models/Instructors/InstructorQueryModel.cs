using Byway.Core.Entities.Enums;
namespace Byway.Core.Models.Instructors;

public class InstructorQueryModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public JobTitle? JobTitle { get; set; }
}
