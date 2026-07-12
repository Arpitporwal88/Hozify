namespace Hozify.Application.Features.Auth.DTOs;

public class VerifyOtpResponseDto
{
    public bool IsNewUser { get; set; }

    public string? RegistrationToken { get; set; }

    public AuthResponseDto? Auth { get; set; }
}