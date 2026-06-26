using Hozify.Domain.Common;

namespace Hozify.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;
}