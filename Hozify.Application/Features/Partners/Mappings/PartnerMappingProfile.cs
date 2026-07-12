using AutoMapper;
using Hozify.Application.Features.Partners.DTOs;
using Hozify.Domain.Entities;

namespace Hozify.Application.Mapping;

public class PartnerMappingProfile : Profile
{
    public PartnerMappingProfile()
    {
        CreateMap<CreatePartnerDto, Partner>();

        CreateMap<UpdatePartnerDto, Partner>();

        CreateMap<Partner, PartnerResponseDto>()
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
    }
}