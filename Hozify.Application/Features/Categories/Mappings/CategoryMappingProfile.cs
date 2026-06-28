using AutoMapper;
using Hozify.Application.Features.Categories.DTOs;
using Hozify.Domain.Entities;

namespace Hozify.Application.Features.Categories.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<Category, CategoryResponseDto>();

        CreateMap<UpdateCategoryDto, Category>();
    }
}