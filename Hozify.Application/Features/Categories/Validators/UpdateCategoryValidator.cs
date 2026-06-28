using FluentValidation;
using Hozify.Application.Features.Categories.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Categories.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage(ValidationMessages.CategoryNameRequired)
            .MinimumLength(3)
                .WithMessage(ValidationMessages.CategoryNameMinLength)
            .MaximumLength(100)
                .WithMessage(ValidationMessages.CategoryNameMaxLength);

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage(ValidationMessages.CategoryDescriptionRequired)
            .MaximumLength(500)
                .WithMessage(ValidationMessages.CategoryDescriptionMaxLength);

        RuleFor(x => x.Icon)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage(ValidationMessages.CategoryIconRequired);
    }
}