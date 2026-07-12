using Hozify.Domain.Enums;

namespace Hozify.Application.Features.Partners.DTOs;

public class CreatePartnerDto
{
    public PartnerType PartnerType { get; set; }

    public Gender Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Experience { get; set; }

    public string Bio { get; set; } = string.Empty;

    public string? ProfileImage { get; set; }

    public string? ReferralCode { get; set; }
}