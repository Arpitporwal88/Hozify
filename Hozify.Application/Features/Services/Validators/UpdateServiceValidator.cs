using FluentValidation;
using Hozify.Application.Features.Services.DTOs;

namespace Hozify.Application.Features.Services.Validators;

public class UpdateServiceValidator : AbstractValidator<UpdateServiceDto>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required.")
            .MinimumLength(3).WithMessage("Service name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Service name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Please select a valid category.");
    }
}