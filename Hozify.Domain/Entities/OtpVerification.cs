using Hozify.Domain.Common;
using Hozify.Domain.Enums;
using System.Net.NetworkInformation;

namespace Hozify.Domain.Entities;

public class OtpVerification : BaseEntity
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string Otp { get; set; } = string.Empty;

    public DateTime ExpiryTime { get; set; }

    public OtpStatus Status { get; set; } = OtpStatus.Pending;

    public int AttemptCount { get; set; } = 0;
}