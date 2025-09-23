using Byway.Core.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace Byway.Core.Dtos.Instructor;

public class InstructorDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public double Rate { get; set; }
    public string? JobTitle { get; set; }
}
