using System.Text.RegularExpressions;

namespace Hozify.Application.Common;

public static class ValidationHelper
{
    public static bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }

    public static bool BeValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^[6-9]\d{9}$");
    }

    public static bool BeValidOtp(string otp)
    {
        return Regex.IsMatch(otp, @"^\d{6}$");
    }
}