using BCrypt.Net;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Application.Features.Auth.Interfaces;
using Hozify.Domain.Entities;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly HozifyDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(
    HozifyDbContext context,
    IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                Message = "Email already exists"
            };
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = 2,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Message = "Registration Successful"
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
        {
            return new AuthResponseDto
            {
                Message = "Invalid Email Please check your Email"
            };
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return new AuthResponseDto
            {
                Message = "Invalid Password Please check your Password"
            };
        }

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Message = "Login Successful"
        };
    }
}