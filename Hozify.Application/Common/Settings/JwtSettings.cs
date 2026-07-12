namespace Hozify.Application.Common.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int AccessTokenExpirationMinutes { get; set; }

    public int RefreshTokenExpirationDays { get; set; }

    public int RegistrationTokenExpirationMinutes { get; set; } = 5;

    public int OtpExpirationMinutes { get; set; }

    public int OtpCooldownSeconds { get; set; }

    public int OtpMaxRequests { get; set; }

    public int OtpRateLimitMinutes { get; set; }
}