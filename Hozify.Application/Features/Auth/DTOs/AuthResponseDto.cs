public class AuthResponseDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public bool IsNewUser { get; set; }

    public bool IsProfileCompleted { get; set; }

    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string? RegistrationToken { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime RefreshTokenExpiresAt { get; set; }
}