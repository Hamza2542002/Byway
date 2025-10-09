using Byway.Core.Entities;
using Byway.Core.Helpers;
using Byway.Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Byway.Application.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JWTConfiguration _jWT;
    public TokenService(UserManager<ApplicationUser> userManager,
        IOptions<JWTConfiguration> jwtOptions,
        RoleManager<IdentityRole<Guid>> roleManager )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jWT = jwtOptions.Value;
    }
    public async Task<JwtSecurityToken> CreateJWTToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("role", role));
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email , user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Iss , "denation-app"),

            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWT.SecurityKey));
        var signInCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
                issuer: _jWT.IssuerIP,
                audience: _jWT.AudienceIP,
                claims: claims,
                expires: DateTime.Now.AddDays(_jWT.DurationInDays),
                 signingCredentials: signInCredentials
            );

        return securityToken;
    }
}
