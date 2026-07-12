using Hozify.Domain.Enums;

namespace Hozify.Application.Features.Partners.DTOs;

public class PartnerResponseDto
{
    // Partner Information
    public int Id { get; set; }

    public int UserId { get; set; }

    public string PartnerCode { get; set; } = string.Empty;

    public PartnerType PartnerType { get; set; }

    // User Information
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    // Personal Information
    public Gender Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Experience { get; set; }

    public string Bio { get; set; } = string.Empty;

    public string? ProfileImage { get; set; }

    // Referral
    public string? ReferralCode { get; set; }

    // Status
    public bool IsVerified { get; set; }

    public bool IsOnline { get; set; }

    public bool IsActive { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}