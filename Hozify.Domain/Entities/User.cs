using Hozify.Domain.Common;

namespace Hozify.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public int? RoleId { get; set; }

    public Role? Role { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Partner? Partner { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}