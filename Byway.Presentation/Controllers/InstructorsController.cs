using Byway.Core.Dtos.Instructor;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models.Instructors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CustomeValidationEception =  Byway.Core.Exceptions.ValidationException;

namespace Byway.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IValidator<InstructorDto> _validator;

        public InstructorsController(IInstructorService instructorService,IValidator<InstructorDto> validator)
        {
            _instructorService = instructorService;
            _validator = validator;
        }
        [HttpGet]
        public async Task<IActionResult> GetInstructors([FromQuery]InstructorQueryModel instructorQueryModel)
        {
            var result = await _instructorService.GetPaginatedInstructors(instructorQueryModel);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructors([FromRoute]Guid id)
        {
            var result = await _instructorService.GetInstructorById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInstructor([FromForm] InstructorDto intructorDto)
        {
            var validationResult = await _validator.ValidateAsync(intructorDto);
            if (!validationResult.IsValid)
            {
                throw new CustomeValidationEception()
                {
                    Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
                };
            }
            var result = await _instructorService.CreateInstructor(intructorDto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor([FromRoute] Guid id, [FromForm] InstructorDto intructorDto)
        {
            var validationResult = await _validator.ValidateAsync(intructorDto);
            if (!validationResult.IsValid)
            {
                throw new CustomeValidationEception()
                {
                    Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
                };
            }
            var result = await _instructorService.UpdateInstructor(id, intructorDto);
            return Ok(result);
        }
    }
}
