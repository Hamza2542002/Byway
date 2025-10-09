using AutoMapper;
using Byway.Core.Dtos.Category;
using Byway.Core.Entities;
using Byway.Core.IRepositories;
using Byway.Core.IServices;

namespace Byway.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<List<CategoryToReturnDto>> GetAllAsync()
    {
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var categories = await categoryRepo.GetAllAsync();
        var data = _mapper.Map<List<CategoryToReturnDto>>(categories);
        data.ForEach(async e => e.CourseCount = await GetCourseCountByCategoryId(e.Id));
        return data;
    }

    private async Task<int> GetCourseCountByCategoryId(Guid id)
    {
        var courseRepo = _unitOfWork.GetRepository<Course>();
        return await courseRepo.GetCountAsync(q => q.Where(c => c.CategoryId == id));
    }
}
