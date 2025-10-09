using Byway.Core.Entities;
using Byway.Core.Entities.Enums;

namespace Byway.Core.Models.Courses;

public class CourseQueryModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Name { get; set; }
    public decimal Cost { get; set; }
    public double TotalHours { get; set; }
    public double Rate { get; set; }
    public string? Level { get; set; }
    public int MinLectureNum { get; set; }
    public int MaxLectureNum { get; set; }
    public string? CategoryId { get; set; }
}
