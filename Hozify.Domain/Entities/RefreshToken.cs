using Hozify.Domain.Common;

namespace Hozify.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; } = false;

    public DateTime? RevokedAt { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public bool IsExpired => ExpiresAt <= AppDateTime.Now;
}