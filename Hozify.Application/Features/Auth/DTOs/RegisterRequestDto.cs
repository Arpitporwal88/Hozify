namespace Hozify.Application.Features.Auth.DTOs;

public class RegisterRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public int RoleId { get; set; }

    public string RegistrationToken { get; set; } = string.Empty;

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}