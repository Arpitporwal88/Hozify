using Hozify.Domain.Enums;

namespace Hozify.Application.Features.Auth.Interfaces;

public interface IOtpService
{
    Task SendOtpAsync(string phoneNumber);

    Task<OtpVerificationStatus> VerifyOtpAsync(string phoneNumber, string otp);
}