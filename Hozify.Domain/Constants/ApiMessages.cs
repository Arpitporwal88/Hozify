namespace Hozify.Domain.Constants;

public static class ApiMessages
{
    // Common
    public const string Success = "Request completed successfully.";
    public const string Failed = "Request failed.";
    public const string ValidationFailed = "Validation failed.";
    public const string InternalServerError = "An unexpected error occurred.";

    // Authentication
    public const string RegistrationSuccess = "Registration successful.";
    public const string LoginSuccess = "Login successful.";
    public const string InvalidCredentials = "Invalid email or password.";

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
}