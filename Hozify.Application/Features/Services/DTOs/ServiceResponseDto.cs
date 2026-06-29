namespace Hozify.Application.Features.Services.DTOs;

public class ServiceResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}