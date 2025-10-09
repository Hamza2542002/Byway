using AutoMapper;
using Byway.Core.Auth;
using Byway.Core.Entities;
using Byway.Core.Exceptions;
using Byway.Core.IServices;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Byway.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, 
        ITokenService tokenService,
        IImageService imageService,
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _imageService = imageService;
        _mapper = mapper;
    }
    public async Task<AuthModel> LoginAsync(LoginDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new BadRequestException("Invalid Login Attempt");

        var token = await _tokenService.CreateJWTToken(user);
        var data = _mapper.Map<UserDto>(user);
        data.Role = (await _userManager.GetRolesAsync(user)).ToList().FirstOrDefault();
        var authModel = new AuthModel
        {
            User = _mapper.Map<UserDto>(user),
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            IsAuthenticated = true,
            ExpiresOn = token.ValidTo,
            Roles = (List<string>)_userManager.GetRolesAsync(user).Result
        };

        return authModel;
    }

    public async Task<AuthModel> RegisterAsync(RegistrationDTO model)
    {
        if (await _userManager.FindByEmailAsync(model?.Email ?? "") != null)
            throw new BadRequestException("Email Already Exist");

        if (await _userManager.FindByNameAsync(model?.Username ?? "") != null)
            throw new BadRequestException("username Already Exist");


        var user = new ApplicationUser
        {
            UserName = model?.Username,
            Email = model?.Email,
            FirstName = model?.FirstName ?? "",
            LastName = model?.LastName ?? "",
        };


        var result = await _userManager.CreateAsync(user, model.Password);


        if (!result.Succeeded)
        {
            var errors = new StringBuilder("");
            foreach (var error in result.Errors)
            {
                errors.AppendLine(error.Description);
            }
            throw new BadRequestException(errors.ToString());
        }

        var res = await _userManager.AddToRoleAsync(user, "user");

        if (!res.Succeeded)
            return new AuthModel { Message = "Can't Add User To role " };

        var token = await _tokenService.CreateJWTToken(user);


        var authModel = new AuthModel
        {
            User = _mapper.Map<UserDto>(user),
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            IsAuthenticated = true,
            ExpiresOn = token.ValidTo,
            Roles = (List<string>)_userManager.GetRolesAsync(user).Result
        };

        return authModel;
    }
}
