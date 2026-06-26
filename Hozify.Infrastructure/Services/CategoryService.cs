using AutoMapper;
using Hozify.Application.Common;
using Hozify.Application.Features.Category.DTOs;
using Hozify.Application.Features.Category.Interfaces;
using Hozify.Domain.Constants;
using Hozify.Domain.Entities;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly HozifyDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService(HozifyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CreateCategoryDto request)
    {
        // Check if category already exists
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == request.Name.Trim() && !c.IsDeleted);

        if (existingCategory != null)
        {
            return ResponseFactory.Failure<CategoryResponseDto>(
                ApiMessages.CategoryAlreadyExists,
                ApiStatusCode.Conflict);
        }

        // DTO -> Entity
        var category = _mapper.Map<Category>(request);

        // Save
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        // Entity -> Response DTO
        var response = _mapper.Map<CategoryResponseDto>(category);

        // Return Success Response
        return ResponseFactory.Success(
            response,
            ApiMessages.CategoryCreated,
            ApiStatusCode.Created);
    }

    public Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<CategoryResponseDto>> UpdateCategoryAsync(int categoryId, UpdateCategoryDto request)
    {
        throw new NotImplementedException();
    }
}