using AutoMapper;
using Byway.Core.Dtos.Course;
using Byway.Core.Entities;
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

    public CourseService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationModel<List<CourseToReturnDto>>> GetAllCoursesAsync(CourseQueryModel courseQueryModel)
    {
        var courseRepository = _unitOfWork.GetRepository<Course>();
        var pageNumber = courseQueryModel.Page;
        var pageSize = courseQueryModel.PageSize;
        var totalHours = courseQueryModel.TotalHours;
        var name = courseQueryModel.Name?.ToLower();
        var cost = courseQueryModel.Cost;
        var rate = courseQueryModel.Rate;
        var level = courseQueryModel.Level?.ToLower();
        var categoryId = courseQueryModel.CategoryId;

        Func<IQueryable<Course>, IQueryable<Course>> query = default!;
        if (!string.IsNullOrEmpty(name))
        {
            query += query => query.Where(c => c.Name!.ToLower() == name);
        }
        if (categoryId != Guid.Empty)
        {
            query += query => query.Where(c => c.CategoryId == categoryId);
        }
        if (totalHours > 0)
        {
            query += query => query.Where(c => c.TotalHours == totalHours);
        }
        if (cost > 0)
        {
            query += query => query.Where(c => c.Cost == cost);
        }
        if (rate > 0)
        {
            query += query => query.Where(c => c.Rate == rate);
        }
        if (!string.IsNullOrEmpty(level))
        {
            query += query => query.Where(c => c.Level.ToString().ToLower() == level);
        }

        var totalRecords = await courseRepository.GetCountAsync(query);

        query += query => query
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.Lectures);

        var courses = await courseRepository.GetAllAsync(query);

        return new PaginationModel<List<CourseToReturnDto>>()
        {
            Data = _mapper.Map<List<CourseToReturnDto>>(courses),
            IsSuccess = true,
            Message = "Courses retrieved Successfully",
            PageNumber = pageNumber,
            PageSize = courses.Count,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
            TotalRecords = totalRecords
        };
    }
}
