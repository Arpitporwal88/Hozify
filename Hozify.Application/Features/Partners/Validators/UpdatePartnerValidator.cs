using FluentValidation;
using Hozify.Application.Common;
using Hozify.Application.Features.Partners.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Partners.Validators;

public class UpdatePartnerValidator : AbstractValidator<UpdatePartnerDto>
{
    public UpdatePartnerValidator()
    {
        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage(ValidationMessages.InvalidGender);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage(ValidationMessages.DateOfBirthRequired)
            .Must(ValidationHelper.BeAtLeast18YearsOld)
            .WithMessage(ValidationMessages.PartnerMustBe18YearsOld);

        RuleFor(x => x.Experience)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ValidationMessages.InvalidExperience);

        RuleFor(x => x.Bio)
            .NotEmpty()
            .WithMessage(ValidationMessages.BioRequired)
            .MaximumLength(500)
            .WithMessage(ValidationMessages.BioMaxLength);

        RuleFor(x => x.ProfileImage)
            .MaximumLength(500)
            .WithMessage(ValidationMessages.ProfileImageMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.ProfileImage));

        RuleFor(x => x.ReferralCode)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.ReferralCodeMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.ReferralCode));
    }
}