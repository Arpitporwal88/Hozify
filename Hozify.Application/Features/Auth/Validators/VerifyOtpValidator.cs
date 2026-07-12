using FluentValidation;
using Hozify.Application.Common;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Auth.Validators;

public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequestDto>
{
    public VerifyOtpValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired)
            .Must(ValidationHelper.BeValidPhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber);

        RuleFor(x => x.Otp)
            .NotEmpty()
            .WithMessage(ValidationMessages.OtpRequired)
            .Must(ValidationHelper.BeValidOtp)
            .WithMessage(ValidationMessages.InvalidOtp);
    }
}