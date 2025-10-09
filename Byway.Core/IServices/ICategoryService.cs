using Byway.Core.Dtos.Category;

namespace Byway.Core.IServices;

public interface ICategoryService
{
    Task<List<CategoryToReturnDto>> GetAllAsync();
}
