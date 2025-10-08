using Byway.Core.Dtos.Course;
using Byway.Core.Models;
using Byway.Core.Models.Courses;

namespace Byway.Core.IServices;

public interface ICourseService
{
    Task<PaginationModel<List<CourseListToReturnDto>>> GetAllCoursesAsync(CourseQueryModel courseQueryModel);
    Task<ServiceResultModel<CourseToReturnDto>> GetCourseByIdAsync(Guid id);
    Task<ServiceResultModel<CourseToReturnDto>> CreateCourseAsync(CourseDto courseDto);
    Task<ServiceResultModel<CourseToReturnDto>> UpdateCourseAsync(Guid id, CourseDto courseDto);
    Task<ServiceResultModel<bool>> DeleteCourseAsync(Guid id);
    Task<ServiceResultModel<List<CourseListToReturnDto>>> Search(CourseSearchModel courseSearchModel);
}
