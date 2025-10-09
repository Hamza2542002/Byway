using Byway.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Byway.Core.IServices;

public interface ITokenService
{
    Task<JwtSecurityToken> CreateJWTToken(ApplicationUser user);
}
