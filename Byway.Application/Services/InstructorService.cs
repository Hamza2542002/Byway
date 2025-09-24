using AutoMapper;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;
using Byway.Core.Entities.Enums;
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
    private readonly IImageService _imageService;

    public InstructorService(IUnitOfWork unitOfWork, IMapper mapper,IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageService = imageService;
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
            PageSize = instructors.Count,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            Data = _mapper.Map<List<InstructorToReturnDto>>(instructors),
            IsSuccess = true,
            Message = "Instructors retrieved successfully"
        };
    }
    public async Task<PaginationModel<List<InstructorToReturnDto>>> GetTopRatedInstructors(InstructorQueryModel instructorQueryModel)
    {
        var instructoRepo = _unitOfWork.GetRepository<Instructor>();
        Func<IQueryable<Instructor>, IQueryable<Instructor>> query = query => query
            .OrderByDescending(i => i.Rate)
            .Skip((instructorQueryModel.Page - 1) * instructorQueryModel.PageSize)
            .Take(instructorQueryModel.PageSize);
        var instructors = await instructoRepo.GetAllAsync(query);
        var totalRecords = instructors.Count;
        return new PaginationModel<List<InstructorToReturnDto>>
        {
            PageNumber = instructorQueryModel.Page,
            PageSize = totalRecords,
            TotalPages = instructors.Count,
            TotalRecords = totalRecords,
            Data = _mapper.Map<List<InstructorToReturnDto>>(instructors),
            IsSuccess = true,
            Message = "Top rated instructors retrieved successfully"
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

        return ServiceResultModel<InstructorToReturnDto>
            .Success(_mapper.Map<InstructorToReturnDto>(instructor), "Instructor retrieved successfully");
    }
    public async Task<ServiceResultModel<InstructorToReturnDto>> CreateInstructor(InstructorDto instructorDto)
    {
        IGenericRepository<Instructor>? instructorRepo = _unitOfWork.GetRepository<Instructor>();

        Instructor? instructor = _mapper.Map<Instructor>(instructorDto);

        await instructorRepo.AddAsync(instructor);

        if (instructorDto.Image is not null)
            instructor.ImageUrl = await _imageService.UploadImageAsync(instructorDto.Image,instructor.Id);
        else
            instructor.ImageUrl = "https://www.istockphoto.com/photos/user-profile";

        await _unitOfWork.CompleteAsync();

        return ServiceResultModel<InstructorToReturnDto>
            .Success(_mapper.Map<InstructorToReturnDto>(instructor), "Instructor Created Successfully");
    }
    public async Task<ServiceResultModel<InstructorToReturnDto>> UpdateInstructor(Guid id, InstructorDto instructorDto)
    {
        var instructorRepo = _unitOfWork.GetRepository<Instructor>();

        var instructor = await instructorRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Instructor not found");

        _mapper.Map(instructorDto,instructor);

        instructorRepo.Update(instructor);

        if (instructorDto.Image is not null)
            instructor.ImageUrl = await _imageService.UpdateImageAsync(instructorDto.Image, id);

        await _unitOfWork.CompleteAsync();

        return ServiceResultModel<InstructorToReturnDto>
            .Success(_mapper.Map<InstructorToReturnDto>(instructor), "Instructor Updated Successfully");
    }
    public async Task<ServiceResultModel<bool>> DeleteInstructor(Guid id)
    {
        var instructorRepo = _unitOfWork.GetRepository<Instructor>();

        var instructor = await instructorRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Instructor not found");

        var courseRepo = _unitOfWork.GetRepository<Course>();

        if(await courseRepo.GetOneAsync(c => c.InstructorId == id) is not null)
            throw new BadRequestException("Instructor cannot be deleted as he / she has courses");

        instructorRepo.Delete(instructor);
        await _imageService.DeleteImageAsync(id);

        await _unitOfWork.CompleteAsync();

        return ServiceResultModel<bool>
            .Success(true, "Instructor Deleted Successfully");
    }
}
