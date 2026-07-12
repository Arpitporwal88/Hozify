using FluentValidation;
using Hozify.Application.Common;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Auth.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired)
            .Must(ValidationHelper.BeValidPhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage(ValidationMessages.FullNameRequired)
            .MinimumLength(3)
            .WithMessage(ValidationMessages.FullNameMinLength)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.FullNameMaxLength);

        RuleFor(x => x.Email)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.EmailMaxLength)
            .EmailAddress()
            .WithMessage(ValidationMessages.InvalidEmail)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.RoleRequired);
    }
}