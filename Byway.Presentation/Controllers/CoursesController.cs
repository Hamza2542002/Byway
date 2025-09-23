using Byway.Core.IServices;
using Byway.Core.Models.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCourses([FromQuery]CourseQueryModel courseQueryModel)
    {
        var result = await _courseService.GetAllCoursesAsync(courseQueryModel);
        return Ok(result);
    }
}
