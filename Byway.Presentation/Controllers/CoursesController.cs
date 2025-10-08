using Byway.Core.Dtos.Course;
using Byway.Core.IServices;
using Byway.Core.Models;
using Byway.Core.Models.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using CustomeValidationEception = Byway.Core.Exceptions.ValidationException;

namespace Byway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IValidator<CourseDto> _validator;

    public CoursesController(ICourseService courseService, IValidator<CourseDto> validator)
    {
        _courseService = courseService;
        _validator = validator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCourses([FromQuery] CourseQueryModel courseQueryModel)
    {
        var result = await _courseService.GetAllCoursesAsync(courseQueryModel);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllCourses([FromRoute] Guid id)
    {
        var result = await _courseService.GetCourseByIdAsync(id);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromForm] CourseDto courseDto, [FromForm] string? lecturesjson)
    {
        courseDto.Lectures = JsonSerializer.Deserialize<List<CourseLectureDto>>(lecturesjson ?? "[]")!;
        var validationResult = await _validator.ValidateAsync(courseDto);
        if (!validationResult.IsValid)
        {
            throw new CustomeValidationEception()
            {
                Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
            };
        }
        var result = await _courseService.CreateCourseAsync(courseDto);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromForm] CourseDto courseDto, [FromForm] string? lecturesjson)
    {
        courseDto.Lectures = JsonSerializer.Deserialize<List<CourseLectureDto>>(lecturesjson ?? "[]")!;
        var validationResult = await _validator.ValidateAsync(courseDto);
        if (!validationResult.IsValid)
        {
            throw new CustomeValidationEception()
            {
                Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
            };
        }
        var result = await _courseService.UpdateCourseAsync(id, courseDto);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        return Ok(result);
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] CourseSearchModel courseSearchModel)
    {
        ServiceResultModel<List<CourseListToReturnDto>>? result = await _courseService.Search(courseSearchModel);
        return Ok(result);
    }
}
