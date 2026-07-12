using FluentValidation;
using Hozify.Application.Common;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Auth.Validators;

public class SendOtpValidator : AbstractValidator<SendOtpRequestDto>
{
    public SendOtpValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired)
            .Must(ValidationHelper.BeValidPhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber);
    }
}