using Byway.Core.Auth;
using Byway.Core.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using CustomeValidationEception = Byway.Core.Exceptions.ValidationException;


namespace Byway.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegistrationDTO> _registerationValidator;
        private readonly IValidator<LoginDTO> _loginValidator;

        public AuthController(IAuthService authService, 
            IValidator<RegistrationDTO> RegisterationValidator,
            IValidator<LoginDTO> LoginValidator)
        {
            _authService = authService;
            _registerationValidator = RegisterationValidator;
            _loginValidator = LoginValidator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO model)
        {
            var validationResult = await _registerationValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new CustomeValidationEception()
                {
                    Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
                };
            }
            var result = await _authService.RegisterAsync(model);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var validationResult = await _loginValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new CustomeValidationEception()
                {
                    Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
                };
            }
            var result = await _authService.LoginAsync(model);

            return Ok(result);
        }
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin(LoginDTO model)
        {
            var validationResult = await _loginValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new CustomeValidationEception()
                {
                    Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)]
                };
            }
            var result = await _authService.LoginAsync(model);

            return Ok(result);
        }

    }
}
