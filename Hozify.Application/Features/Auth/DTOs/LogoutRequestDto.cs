namespace Hozify.Application.Features.Auth.DTOs;

public class LogoutRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}