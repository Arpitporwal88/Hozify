namespace Hozify.Domain.Constants;

public static class ValidationMessages
{
    // =========================
    // Auth Validation Messages
    // =========================
    public const string PhoneNumberRequired = "Phone number is required.";
    public const string InvalidPhoneNumber = "Please enter a valid 10-digit mobile number.";
    public const string OtpRequired = "OTP is required.";
    public const string InvalidOtp = "OTP must be exactly 6 digits.";
    public const string FullNameRequired = "Full name is required.";
    public const string FullNameMinLength = "Full name must be at least 3 characters.";
    public const string FullNameMaxLength = "Full name cannot exceed 100 characters.";
    public const string EmailMaxLength = "Email cannot exceed 100 characters.";
    public const string InvalidEmail = "Please enter a valid email address.";
    public const string RoleRequired = "Role is required.";
    public const string RefreshTokenRequired = "Refresh token is required.";

    // Category validation messages
    public const string CategoryNameRequired = "Category name is required.";
    public const string CategoryNameMinLength = "Category name must be at least 3 characters.";
    public const string CategoryNameMaxLength = "Category name cannot exceed 100 characters.";
    public const string CategoryDescriptionRequired = "Category description is required.";
    public const string CategoryDescriptionMaxLength = "Category description cannot exceed 500 characters.";
    public const string CategoryIconRequired = "Category icon is required.";


    // Partner validation messages
    public const string UserIdRequired = "User Id is required.";
    public const string InvalidPartnerType = "Invalid partner type.";
    public const string InvalidGender = "Invalid gender.";
    public const string DateOfBirthRequired = "Date of birth is required.";
    public const string PartnerMustBe18YearsOld = "Partner must be at least 18 years old.";
    public const string InvalidExperience = "Experience must be greater than or equal to 0.";
    public const string BioRequired = "Bio is required.";
    public const string BioMaxLength = "Bio cannot exceed 500 characters.";
    public const string ProfileImageMaxLength = "Profile image cannot exceed 500 characters.";
    public const string ReferralCodeMaxLength = "Referral code cannot exceed 20 characters.";
}