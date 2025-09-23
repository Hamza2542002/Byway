using Byway.Core.Entities;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models.Instructors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetInstructors([FromQuery]InstructorQueryModel instructorQueryModel)
        {
            var result = await _instructorService.GetPaginatedInstructors(instructorQueryModel);
            return Ok(result.Data);
        }
    }
}
