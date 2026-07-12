using Hozify.Domain.Common;

namespace Hozify.Domain.Entities;

public class Customer : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public string? ReferralCode { get; set; }
}