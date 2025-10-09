using Byway.Core.Auth;

namespace Byway.Core.IServices;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegistrationDTO model);
    Task<AuthModel> LoginAsync(LoginDTO model);
}
