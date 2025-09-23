using AutoMapper;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;
using Byway.Core.Entities.Enums;
using Byway.Core.IRepositories;
using Byway.Core.IServices;
using Byway.Core.Models;
using Byway.Persestance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services;

public class InstructorService : IInstructorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InstructorService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationModel<InstructorToReturnDto>> GetPaginatedInstructors(int pageNumber, int pageSize,string? search = null)
    {
        var instructoRepo = _unitOfWork.GetRepository<Instructor>();
        Func<IQueryable<Instructor>,IQueryable<Instructor>> query = query => query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        if (!string.IsNullOrEmpty(search))
        {
            query += (q => q.Where(i => (i != null && i.Name != null) && (i.Name.Contains(search) || i.Description.Contains(search))));
        }
        var instructors = await instructoRepo.GetAllAsync(query);
        var totalRecords = await instructoRepo.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        return new PaginationModel<InstructorToReturnDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            Data = _mapper.Map<List<InstructorToReturnDto>>(instructors),
            Success = true,
            Message = "Instructors retrieved successfully"
        };
    }
    public Task<InstructorToReturnDto> CreateInstructor(InstructorDto instructorDto)
    {
        throw new NotImplementedException();
    }

    public Task<InstructorToReturnDto> GetInstructor(Guid id)
    {
        throw new NotImplementedException();
    }
}
