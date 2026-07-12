using FluentValidation;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Auth.Validators;

public class LogoutValidator : AbstractValidator<LogoutRequestDto>
{
    public LogoutValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(ValidationMessages.RefreshTokenRequired);
    }
}