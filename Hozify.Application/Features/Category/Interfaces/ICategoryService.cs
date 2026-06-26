using Hozify.Application.Common;
using Hozify.Application.Features.Category.DTOs;

namespace Hozify.Application.Features.Category.Interfaces;

public interface ICategoryService
{
    Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CreateCategoryDto request);

    Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync();

    Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId);

    Task<ApiResponse<CategoryResponseDto>> UpdateCategoryAsync(int categoryId, UpdateCategoryDto request);

    Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId);
}