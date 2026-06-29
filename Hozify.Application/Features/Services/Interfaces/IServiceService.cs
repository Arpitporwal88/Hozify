using Hozify.Application.Common;
using Hozify.Application.Features.Services.DTOs;

namespace Hozify.Application.Features.Services.Interfaces;

public interface IServiceService
{
    Task<ApiResponse<ServiceResponseDto>> CreateServiceAsync(CreateServiceDto request);

    Task<ApiResponse<List<ServiceResponseDto>>> GetAllServicesAsync();

    Task<ApiResponse<ServiceResponseDto>> GetServiceByIdAsync(int serviceId);

    Task<ApiResponse<ServiceResponseDto>> UpdateServiceAsync(int serviceId, UpdateServiceDto request);

    Task<ApiResponse<bool>> DeleteServiceAsync(int serviceId);

    Task<ApiResponse<bool>> RestoreServiceAsync(int serviceId);
}