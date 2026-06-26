using Hozify.Application.Features.Category.DTOs;
using Hozify.Application.Features.Category.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hozify.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto request)
    {
        var result = await _categoryService.CreateCategoryAsync(request);

        return StatusCode(result.StatusCode, result);
    }
}