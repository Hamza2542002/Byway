using AutoMapper;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;
using Byway.Core.Exceptions;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models;
using Byway.Core.Models.Instructors;
using Microsoft.EntityFrameworkCore;

namespace Byway.Application.Services;

public class InstructorService : IInstructorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InstructorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationModel<List<InstructorToReturnDto>>> GetPaginatedInstructors(InstructorQueryModel instructorQueryModel)
    {
        var instructoRepo = _unitOfWork.GetRepository<Instructor>();

        var pageNumber = instructorQueryModel.Page < 1 ? 1 : instructorQueryModel.Page;
        var pageSize = instructorQueryModel.PageSize < 1 ? 10 : instructorQueryModel.PageSize;
        var search = instructorQueryModel.Search;
        var jobTitle = instructorQueryModel.JobTitle ?? null;

        Func<IQueryable<Instructor>, IQueryable<Instructor>> query = query => query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        if (!string.IsNullOrEmpty(search))
        {
            query += (q => q.Where(i => (i != null && i.Name != null) && (i.Name.Contains(search) || i.Description.Contains(search))));
        }
        if (jobTitle.HasValue)
        {
            query += (q => q.Where(i => i.JobTitle == jobTitle));
        }

        var instructors = await instructoRepo.GetAllAsync(query);
        var totalRecords = await instructoRepo.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        return new PaginationModel<List<InstructorToReturnDto>>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            Data = _mapper.Map<List<InstructorToReturnDto>>(instructors),
            IsSuccess = true,
            Message = "Instructors retrieved successfully"
        };
    }
    public async Task<ServiceResultModel<InstructorToReturnDto>> GetInstructorById(Guid id)
    {
        if (id == Guid.Empty) throw new BadRequestException("Invalid ID");

        IGenericRepository<Instructor>? instructoRepo = _unitOfWork.GetRepository<Instructor>();
        Func<IQueryable<Instructor>, IQueryable<Instructor>> query =
            q => q.Include(i => i.Courses!).ThenInclude(c => c.Category);

        Instructor? instructor = await instructoRepo.GetByIdAsync(id, query: query)
            ?? throw new NotFoundException("Instructor not found");

        return new ServiceResultModel<InstructorToReturnDto>
        {
            Data = _mapper.Map<InstructorToReturnDto>(instructor),
            IsSuccess = true,
            Message = "Instructor retrieved successfully"
        };
    }
    public Task<InstructorToReturnDto> CreateInstructor(InstructorDto instructorDto)
    {
        throw new NotImplementedException();
    }
}
