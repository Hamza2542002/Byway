using Byway.Application.Services;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;
using Byway.Core.Helpers;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Profiles;
using Byway.Core.Validators.Instructors;
using Byway.Persestance;
using Byway.Persestance.Data;
using Byway.Persestance.Repositories;
using Byway.Presentation.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<AddInstructorValidator>();
        builder.Services.AddScoped<IValidator<InstructorDto>, AddInstructorValidator>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.Configure<CloudinatuConfiguration>(builder.Configuration.GetSection("CloudinarySettings"));
        
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
        
        builder.Services.AddIdentity<ApplicationUser,IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddOpenApi();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
