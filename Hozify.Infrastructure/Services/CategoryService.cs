using AutoMapper;
using Hozify.Application.Common;
using Hozify.Application.Features.Categories.DTOs;
using Hozify.Application.Features.Categories.Interfaces;
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

    public async Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync()
    {
        //throw new Exception("Testing Exception Middleware");
        var categories = await _context.Categories
            .Where(c => !c.IsDeleted)
            .ToListAsync();

        var response = _mapper.Map<List<CategoryResponseDto>>(categories);

        return ResponseFactory.Success(
            response,
            ApiMessages.Success,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);

        if (category == null)
        {
            return ResponseFactory.Failure<CategoryResponseDto>(
                ApiMessages.CategoryNotFound,
                ApiStatusCode.NotFound);
        }

        var response = _mapper.Map<CategoryResponseDto>(category);

        return ResponseFactory.Success(
            response,
            ApiMessages.Success,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<CategoryResponseDto>> UpdateCategoryAsync(int categoryId, UpdateCategoryDto request)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);

        if (category == null)
        {
            return ResponseFactory.Failure<CategoryResponseDto>(
                ApiMessages.CategoryNotFound,
                ApiStatusCode.NotFound);
        }

        request.Name = request.Name.Trim();

        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.Name == request.Name &&
                c.Id != categoryId &&
                !c.IsDeleted);

        if (existingCategory != null)
        {
            return ResponseFactory.Failure<CategoryResponseDto>(
                ApiMessages.CategoryAlreadyExists,
                ApiStatusCode.Conflict);
        }

        _mapper.Map(request, category);

        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var response = _mapper.Map<CategoryResponseDto>(category);

        return ResponseFactory.Success(
            response,
            ApiMessages.CategoryUpdated,
            ApiStatusCode.OK);
    }
    public async Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);

        if (category == null)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.CategoryNotFound,
                ApiStatusCode.NotFound);
        }

        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ResponseFactory.Success(
            true,
            ApiMessages.CategoryDeleted,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<bool>> RestoreCategoryAsync(int categoryId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.CategoryNotFound,
                ApiStatusCode.NotFound);
        }

        if (!category.IsDeleted)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.CategoryAlreadyActive,
                ApiStatusCode.BadRequest);
        }

        category.IsDeleted = false;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ResponseFactory.Success(
            true,
            ApiMessages.CategoryRestored,
            ApiStatusCode.OK);
    }

}