using Byway.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }

}
