using Byway.Core.Dtos.Course;
using Byway.Core.Models;
using Byway.Core.Models.Courses;

namespace Byway.Core.IServices;

public interface ICourseService
{
    Task<PaginationModel<List<CourseToReturnDto>>> GetAllCoursesAsync(CourseQueryModel courseQueryModel);
}
