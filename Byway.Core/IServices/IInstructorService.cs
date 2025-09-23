using Byway.Core.Dtos.Instructor;
using Byway.Core.Models;

namespace Byway.Core.IServices;

public interface IInstructorService
{
    Task<PaginationModel<InstructorToReturnDto>> GetPaginatedInstructors(int pageNumber, int pageSize, string? search = null);
    Task<InstructorToReturnDto> GetInstructor(Guid id);
    Task<InstructorToReturnDto> CreateInstructor(InstructorDto instructorDto);
}
