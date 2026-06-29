using FluentValidation;
using Hozify.Application.Features.Services.DTOs;

namespace Hozify.Application.Features.Services.Validators;

public class CreateServiceValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Service name is required.")
            .MinimumLength(3).WithMessage("Service name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Service name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Please select a valid category.");
    }
}