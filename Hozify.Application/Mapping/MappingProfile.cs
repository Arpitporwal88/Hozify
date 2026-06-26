using AutoMapper;
using Hozify.Application.Features.Category.DTOs;
using Hozify.Domain.Entities;

namespace Hozify.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<Category, CategoryResponseDto>();

        CreateMap<UpdateCategoryDto, Category>();
    }
}