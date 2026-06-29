using AutoMapper;
using Hozify.Application.Features.Services.DTOs;
using ServiceEntity = Hozify.Domain.Entities.Service;

namespace Hozify.Application.Features.Services.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<CreateServiceDto, ServiceEntity>();

        CreateMap<UpdateServiceDto, ServiceEntity>();

        CreateMap<ServiceEntity, ServiceResponseDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name));
    }
}