using AutoMapper;
using Byway.Core.Dtos.Course;
using Byway.Core.Entities;
using Byway.Core.Exceptions;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models;
using Byway.Core.Models.Courses;
using Microsoft.EntityFrameworkCore;

namespace Byway.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper,IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<PaginationModel<List<CourseListToReturnDto>>> GetAllCoursesAsync(CourseQueryModel courseQueryModel)
    {
        var courseRepository = _unitOfWork.GetRepository<Course>();
        var pageNumber = courseQueryModel.PageNumber;
        var pageSize = courseQueryModel.PageSize;
        var totalHours = courseQueryModel.TotalHours;
        var name = courseQueryModel.Name;
        var cost = courseQueryModel.Cost;
        var rate = courseQueryModel.Rate;
        var level = courseQueryModel.Level?.ToLower();
        var categoryId = courseQueryModel.CategoryId;

        Func<IQueryable<Course>, IQueryable<Course>> query = q =>
        {
            if (cost > 0)
                q = q.Where(c => c.Cost <= cost);

            if (rate > 0)
                q = q.Where(c => c.Rate <= rate);

            if (!string.IsNullOrEmpty(name))
                q = q.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(level))
                q = q.Where(c => c.Level.ToString().ToLower() == level);

            if (categoryId != Guid.Empty)
                q = q.Where(c => c.CategoryId == categoryId);

            if (totalHours > 0)
                q = q.Where(c => c.TotalHours <= totalHours);

            return q
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Lectures)
                .Include(c => c.Instructor)
                .Include(c => c.Category);
        };

        var totalRecords = await courseRepository.GetCountAsync(query);

        var courses = await courseRepository.GetAllAsync(query);

        return new PaginationModel<List<CourseListToReturnDto>>()
        {
            Data = _mapper.Map<List<CourseListToReturnDto>>(courses),
            IsSuccess = true,
            Message = "Courses retrieved Successfully",
            PageNumber = pageNumber,
            PageSize = courses.Count,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
            TotalRecords = totalRecords
        };
    }

    public async Task<ServiceResultModel<CourseToReturnDto>> GetCourseByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new BadRequestException("Invalid ID");

        var courseRepository = _unitOfWork.GetRepository<Course>();

        Func<IQueryable<Course>, IQueryable<Course>> query =
                q => q.Include(c => c.Category)
                    .Include(c => c.Lectures)
                    .Include(c => c.Instructor);

        var course = await courseRepository.GetByIdAsync(id, query: query)
            ?? throw new NotFoundException("Course not found");

        return ServiceResultModel<CourseToReturnDto>.Success(
            _mapper.Map<CourseToReturnDto>(course), "Course retrieved successfully");
    }

    public async Task<ServiceResultModel<CourseToReturnDto>> CreateCourseAsync(CourseDto courseDto)
    {
        var courseRepository = _unitOfWork.GetRepository<Course>();
        var course = _mapper.Map<Course>(courseDto);

        await courseRepository.AddAsync(course);

        if(courseDto.Image is not null)
            course.ImageUrl = await _imageService.UploadImageAsync(courseDto.Image, course.Id);
        var result = await _unitOfWork.CompleteAsync();

        if (result <= 0)
            throw new Exception("Failed to create course");

        var newCourse = await courseRepository.GetByIdAsync(course.Id,
            q => q.Include(c => c.Category)
                .Include(c => c.Lectures)
                .Include(c => c.Instructor));

        return ServiceResultModel<CourseToReturnDto>.Success(
            _mapper.Map<CourseToReturnDto>(course), "Course created successfully");
    }

    public async Task<ServiceResultModel<CourseToReturnDto>> UpdateCourseAsync(Guid id, CourseDto courseDto)
    {
        if (id == Guid.Empty) throw new BadRequestException("Invalid ID");

        var courseRepository = _unitOfWork.GetRepository<Course>();

        var existingCourse = await courseRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Course not found");

        _mapper.Map(courseDto, existingCourse);

        courseRepository.Update(existingCourse);

        if(courseDto.Image is not null)
            existingCourse.ImageUrl = await _imageService.UpdateImageAsync(courseDto.Image, existingCourse.Id);

        var result = await _unitOfWork.CompleteAsync();

        if (result <= 0)
            throw new Exception("Failed to update course");

        var updatedCourse = await courseRepository.GetByIdAsync(id,
            q => q.Include(c => c.Category)
                .Include(c => c.Lectures)
                .Include(c => c.Instructor));

        return ServiceResultModel<CourseToReturnDto>.Success(
            _mapper.Map<CourseToReturnDto>(updatedCourse), "Course updated successfully");
    }

    public async Task<ServiceResultModel<bool>> DeleteCourseAsync(Guid id)
    {
        var courseRepository = _unitOfWork.GetRepository<Course>();

        var existingCourse = await courseRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Course not found");

        var enrollmentInstructor = _unitOfWork.GetRepository<CourseEnrollment>();
        var enrollments = await enrollmentInstructor.GetAllAsync(q => q.Where(e => e.CourseId == id));
        if(enrollments.Any())
            throw new BadRequestException("Cannot delete course with active enrollments");
        courseRepository.Delete(existingCourse);

        if(existingCourse.ImageUrl is not null)
            await _imageService.DeleteImageAsync(existingCourse.Id);

        var result = await _unitOfWork.CompleteAsync();

        if (result <= 0)
            throw new Exception("Failed to delete course");
        return ServiceResultModel<bool>.Success(true, "Course deleted successfully");
    }
}
