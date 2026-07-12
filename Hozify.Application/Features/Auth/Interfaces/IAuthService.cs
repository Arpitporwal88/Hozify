using Hozify.Application.Features.Auth.DTOs;

namespace Hozify.Application.Features.Auth.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<object>> SendOtpAsync(SendOtpRequestDto request);

    Task<ApiResponse<VerifyOtpResponseDto>> VerifyOtpAsync(VerifyOtpRequestDto request);

    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);

    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);

    Task<ApiResponse<object>> LogoutAsync(LogoutRequestDto request);
}