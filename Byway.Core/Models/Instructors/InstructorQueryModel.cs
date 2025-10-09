using Byway.Core.Entities.Enums;
namespace Byway.Core.Models.Instructors;

public class InstructorQueryModel
{
    public int Page { get; set; }
    public int PageSize { get; set; } 
    public string? Search { get; set; }
    public JobTitle? JobTitle { get; set; }
    public int Rate { get; set; }
}
