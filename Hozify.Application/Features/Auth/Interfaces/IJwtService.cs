using Hozify.Domain.Entities;

namespace Hozify.Application.Features.Auth.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}