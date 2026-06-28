namespace Hozify.Domain.Constants;

public static class ValidationMessages
{
    // Category
    public const string CategoryNameRequired = "Category name is required.";
    public const string CategoryNameMinLength = "Category name must be at least 3 characters.";
    public const string CategoryNameMaxLength = "Category name cannot exceed 100 characters.";

    public const string CategoryDescriptionRequired = "Category description is required.";
    public const string CategoryDescriptionMaxLength = "Category description cannot exceed 500 characters.";

    public const string CategoryIconRequired = "Category icon is required.";
}