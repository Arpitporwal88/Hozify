using FluentValidation;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Domain.Constants;

namespace Hozify.Application.Features.Auth.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(ValidationMessages.RefreshTokenRequired);
    }
}