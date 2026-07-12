using Hozify.Domain.Common;
using Hozify.Domain.Enums;

namespace Hozify.Domain.Entities;

public class Partner : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string PartnerCode { get; set; } = string.Empty;

    public PartnerType PartnerType { get; set; }

    public Gender Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Experience { get; set; }

    public string Bio { get; set; } = string.Empty;

    public string? ProfileImage { get; set; }

    public string? ReferralCode { get; set; }

    public bool IsVerified { get; set; } = false;

    public bool IsOnline { get; set; } = false;

    public bool IsActive { get; set; } = true;
}