namespace Hozify.Domain.Enums;

public enum OtpVerificationStatus
{
    Success = 1,
    InvalidOtp = 2,
    Expired = 3,
    MaxAttemptsExceeded = 4
}