using Byway.Core.Dtos.Enrollments;
using Byway.Core.Entities;
using Byway.Core.Exceptions;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Byway.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public EnrollmentService(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    public async Task<ServiceResultModel<EnrollmentDto>> CreateEnrollment(EnrollmentDto enrollmentDto)
    {
        var courseRepository = _unitOfWork.GetRepository<Course>();

        var course = await courseRepository.GetByIdAsync(enrollmentDto.CourseId) 
            ?? throw new BadRequestException("Course not found");

        var user = await _userManager.FindByIdAsync(enrollmentDto.UserId.ToString())
            ?? throw new BadRequestException("User not found");

        if (enrollmentDto.CourseId == Guid.Empty || enrollmentDto.UserId == Guid.Empty)
            throw new BadRequestException("CourseId and UserId are required");

        var enrollmentRepository = _unitOfWork.GetRepository<CourseEnrollment>();
        var enrollment = await enrollmentRepository.GetOneAsync(e => e.CourseId == enrollmentDto.CourseId && e.UserId == enrollmentDto.UserId);
        if(enrollment is not null)
            throw new BadRequestException("User already enrolled in this course");

        enrollment = new CourseEnrollment()
        {
            CourseId = enrollmentDto.CourseId,
            UserId = enrollmentDto.UserId,
        };

        await enrollmentRepository.AddAsync(enrollment);
        var result = await _unitOfWork.CompleteAsync();
        if (result <= 0)
            throw new Exception("Failed to create enrollment");
        return ServiceResultModel<EnrollmentDto>
            .Success(enrollmentDto, "Enrollment created successfully");

        throw new NotImplementedException();
    }
}
