using AutoMapper;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Entities;

namespace Hozify.Application.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        // Register -> User
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Partner, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore());

        // User -> Auth Response
        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));
    }
}