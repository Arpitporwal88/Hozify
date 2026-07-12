using Hozify.Application.Common;
using Hozify.Application.Features.Partners.DTOs;

namespace Hozify.Application.Features.Partners.Interfaces;

public interface IPartnerService
{
    Task<ApiResponse<PartnerResponseDto>> CreatePartnerProfileAsync(int userId, CreatePartnerDto request);

    Task<ApiResponse<PartnerResponseDto>> GetMyProfileAsync(int userId);

    Task<ApiResponse<PartnerResponseDto>> UpdateMyProfileAsync(int userId, UpdatePartnerDto request);

}