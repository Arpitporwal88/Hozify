namespace Hozify.Domain.Constants;

public static class ApiMessages
{
    // Common
    public const string Success = "Request completed successfully.";
    public const string Failed = "Request failed.";
    public const string ValidationFailed = "Validation failed.";
    public const string InternalServerError = "An unexpected error occurred.";
    public const string InvalidToken = "Invalid or expired token.";

    // Authentication
    public const string OtpSentSuccessfully = "OTP sent successfully.";
    public const string AccountInactive = "Your account has been deactivated. Please contact support.";
    public const string RegistrationSuccess = "Registration successful.";
    public const string LoginSuccess = "Login successful.";
    public const string InvalidCredentials = "Invalid email or password.";
    public const string InvalidOtp = "Invalid OTP.";
    public const string RegistrationRequired = "Registration required.";
    public const string RefreshTokenGenerated = "Token refreshed successfully.";
    public const string LogoutSuccess = "Logged out successfully.";
    public const string InvalidRole = "Invalid role selected.";
    public const string UserAlreadyExists = "User already exists.";
    public const string UserNotCreate = "Failed to create user.";
    public const string OtpExpired = "OTP has expired.";
    public const string MaxOtpAttemptsExceeded = "Maximum OTP attempts exceeded. Please request a new OTP.";
    public const string OtpCooldown = "Please wait before requesting another OTP.";



    // Category
    public const string CategoryCreated = "Category created successfully.";
    public const string CategoryUpdated = "Category updated successfully.";
    public const string CategoryDeleted = "Category deleted successfully.";
    public const string CategoryNotFound = "Category not found.";
    public const string CategoryAlreadyExists = "Category already exists.";
    public const string CategoryAlreadyActive = "Category is already active.";
    public const string CategoryRestored = "Category restored successfully.";

    // Service
    public const string ServiceCreated = "Service created successfully.";
    public const string ServiceUpdated = "Service updated successfully.";
    public const string ServiceDeleted = "Service deleted successfully.";
    public const string ServiceNotFound = "Service not found.";
    public const string ServiceAlreadyExists = "Service already exists.";
    public const string ServiceAlreadyActive = "Service is already active.";
    public const string ServiceRestored = "Service restored successfully.";
    public const string InvalidCategory = "Selected category does not exist.";


    // Partner
    public const string PartnerCreated = "Partner created successfully.";
    public const string PartnerUpdated = "Partner updated successfully.";
    public const string PartnerNotFound = "Partner not found.";
    public const string PartnerAlreadyExists = "Partner already exists.";
    public const string UserNotFound = "User not found.";
    public const string UserAlreadyRegisteredAsPartner = "User is already registered as a partner.";
    public const string InvalidPartnerRole = "Selected user is not assigned the Partner role.";
}