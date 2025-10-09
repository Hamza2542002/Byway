using Byway.Core.Entities;
using Byway.Core.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Byway.Core.Dtos.Course;

public class CourseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public decimal Cost { get; set; }
    public double TotalHours { get; set; }
    public double Rate { get; set; }
    public string? Certification { get; set; }
    public string? Level { get; set; }
    public Guid InstructorId { get; set; }
    public Guid CategoryId { get; set; }
    public List<CourseLectureDto>? Lectures { get; set; }
    
}