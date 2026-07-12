using System.Security.Claims;
using Hozify.Domain.Entities;

namespace Hozify.Application.Features.Auth.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    int GetUserIdFromToken(ClaimsPrincipal principal);

    string GenerateRegistrationToken(string phoneNumber);

    string? ValidateRegistrationToken(string token);
}