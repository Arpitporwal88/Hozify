using Hozify.Domain.Common;

namespace Hozify.Domain.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign Key
    public int CategoryId { get; set; }

    // Navigation Property
    public Category Category { get; set; } = null!;
}