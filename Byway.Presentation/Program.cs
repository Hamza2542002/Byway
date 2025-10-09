using Byway.Application.Services;
using Byway.Core.Auth;
using Byway.Core.Dtos.Course;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;
using Byway.Core.Helpers;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Profiles;
using Byway.Core.Validators.Auth;
using Byway.Core.Validators.Course;
using Byway.Core.Validators.Instructors;
using Byway.Persestance;
using Byway.Persestance.Data;
using Byway.Persestance.Repositories;
using Byway.Presentation.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Byway.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(typeof(MappingProfiles));
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IInstructorService, InstructorService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<AddInstructorValidator>();
        builder.Services.AddScoped<IValidator<InstructorDto>, AddInstructorValidator>();
        builder.Services.AddScoped<IValidator<RegistrationDTO>, RegistrationValidator>();
        builder.Services.AddScoped<IValidator<LoginDTO>, LoginValidator>();
        builder.Services.AddScoped<IValidator<CourseDto>, CourseValidator>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.Configure<CloudinatuConfiguration>(builder.Configuration.GetSection("CloudinarySettings"));
        
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        builder.Services.AddOpenApi();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWT"));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = false;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:IssureIP"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:AudienceIP"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"] ?? "wjhehsakdhjkashd22yewuiouioqjasdkm,"))
            };

        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
