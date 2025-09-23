using Byway.Core.Dtos.Instructor;
using Byway.Core.Models;
using Byway.Core.Models.Instructors;

namespace Byway.Core.IServices;

public interface IInstructorService
{
    Task<PaginationModel<List<InstructorToReturnDto>>> GetPaginatedInstructors(InstructorQueryModel instructorQueryModel);
    Task<ServiceResultModel<InstructorToReturnDto>> GetInstructorById(Guid id);
    Task<InstructorToReturnDto> CreateInstructor(InstructorDto instructorDto);
}
