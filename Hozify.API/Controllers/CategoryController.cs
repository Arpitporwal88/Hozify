using Hozify.Application.Features.Categories.DTOs;
using Hozify.Application.Features.Categories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hozify.API.Controllers;

[Route("api/v1/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto request)
    {
        var result = await _categoryService.UpdateCategoryAsync(id, request);

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:int}/restore")]
    public async Task<IActionResult> RestoreCategory(int id)
    {
        var result = await _categoryService.RestoreCategoryAsync(id);

        return StatusCode(result.StatusCode, result);
    }
}