namespace Hozify.Domain.Constants;

public static class ApiMessages
{
    // Common
    public const string Success = "Request completed successfully.";
    public const string Failed = "Request failed.";

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
}