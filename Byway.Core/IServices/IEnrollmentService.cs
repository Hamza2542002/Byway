using Byway.Core.Dtos.Enrollments;
using Byway.Core.Models;

namespace Byway.Core.IServices;

public interface IEnrollmentService
{
    Task<ServiceResultModel<EnrollmentDto>> CreateEnrollment(EnrollmentDto enrollmentDto);
}
