namespace Hozify.Application.Features.Auth.DTOs;

public class VerifyOtpRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string Otp { get; set; } = string.Empty;

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}