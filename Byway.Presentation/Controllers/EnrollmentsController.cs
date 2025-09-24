using Byway.Core.Dtos.Enrollments;
using Byway.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentDto enrollmentDto)
        {
            var result = await _enrollmentService.CreateEnrollment(enrollmentDto);
            return Ok(result);
        }
    }
}
