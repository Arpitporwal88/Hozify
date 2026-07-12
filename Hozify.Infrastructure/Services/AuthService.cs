using AutoMapper;
using Hozify.Application.Common;
using Hozify.Application.Common.Settings;
using Hozify.Application.Features.Auth.DTOs;
using Hozify.Application.Features.Auth.Interfaces;
using Hozify.Domain.Common;
using Hozify.Domain.Constants;
using Hozify.Domain.Entities;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hozify.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly HozifyDbContext _context;
    private readonly IOtpService _otpService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        HozifyDbContext context,
        IOtpService otpService,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _otpService = otpService;
        _jwtService = jwtService;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<ApiResponse<object>> SendOtpAsync(SendOtpRequestDto request)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);

        if (user != null && !user.IsActive)
        {
            return ResponseFactory.Failure<object>(
                ApiMessages.AccountInactive,
                ApiStatusCode.Forbidden);
        }

        await _otpService.SendOtpAsync(request.PhoneNumber);

        return ResponseFactory.Success<object>(
            null,
            ApiMessages.OtpSentSuccessfully);
    }

    public async Task<ApiResponse<VerifyOtpResponseDto>> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        var otpStatus = await _otpService.VerifyOtpAsync(request.PhoneNumber, request.Otp);
        switch (otpStatus)
        {
            case OtpVerificationStatus.InvalidOtp:
                return ResponseFactory.Failure<VerifyOtpResponseDto>(
                    ApiMessages.InvalidOtp,
                    ApiStatusCode.BadRequest);

            case OtpVerificationStatus.Expired:
                return ResponseFactory.Failure<VerifyOtpResponseDto>(
                    ApiMessages.OtpExpired,
                    ApiStatusCode.BadRequest);

            case OtpVerificationStatus.MaxAttemptsExceeded:
                return ResponseFactory.Failure<VerifyOtpResponseDto>(
                    ApiMessages.MaxOtpAttemptsExceeded,
                    ApiStatusCode.BadRequest);

            case OtpVerificationStatus.Success:
                break;
        }

        var user = await GetUserByPhoneNumberAsync(request.PhoneNumber);

        // New User
        if (user == null)
        {
            var registrationToken = _jwtService.GenerateRegistrationToken(request.PhoneNumber);

            return ResponseFactory.Success(
                new VerifyOtpResponseDto
                {
                    IsNewUser = true,
                    RegistrationToken = registrationToken
                },
                ApiMessages.RegistrationRequired);
        }

        // Inactive User
        if (!user.IsActive)
        {
            return ResponseFactory.Failure<VerifyOtpResponseDto>(
                ApiMessages.AccountInactive,
                ApiStatusCode.Forbidden);
        }

        //await RevokeAllRefreshTokensAsync(user.Id);
        var auth = await GenerateAuthResponseAsync(user, request.DeviceId, request.DeviceName);

        return ResponseFactory.Success(
            new VerifyOtpResponseDto
            {
                IsNewUser = false,
                Auth = auth
            },
            ApiMessages.LoginSuccess);
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        var phoneNumber = _jwtService.ValidateRegistrationToken(request.RegistrationToken);


        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.InvalidToken,
                ApiStatusCode.Unauthorized);
        }

        if (phoneNumber != request.PhoneNumber)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.InvalidToken,
                ApiStatusCode.Unauthorized);
        }

        var existingUser = await GetUserByPhoneNumberAsync(request.PhoneNumber);

        if (existingUser != null)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.UserAlreadyExists,
                ApiStatusCode.Conflict);
        }

        var role = await GetRoleAsync(request.RoleId);

        if (role == null)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.InvalidRole,
                ApiStatusCode.BadRequest);
        }

        if (role.Name != RoleConstants.Customer && role.Name != RoleConstants.Partner)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.InvalidRole,
                ApiStatusCode.BadRequest);
        }

        var user = new User
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            RoleId = request.RoleId,
            IsActive = true
        };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        // Reload user with Role
        user = await GetUserByPhoneNumberAsync(request.PhoneNumber)
            ?? throw new InvalidOperationException(ApiMessages.UserNotCreate);

        //await RevokeAllRefreshTokensAsync(user.Id);
        var response = await GenerateAuthResponseAsync(user, request.DeviceId, request.DeviceName);
        return ResponseFactory.Success(
            response,
            ApiMessages.RegistrationSuccess);
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(x => x.User)
                .ThenInclude(x => x.Role)
            .Include(x => x.User)
                .ThenInclude(x => x.Customer)
            .Include(x => x.User)
                .ThenInclude(x => x.Partner)
            .FirstOrDefaultAsync(x =>
                x.Token == request.RefreshToken &&
                !x.IsRevoked);

        if (refreshToken == null || refreshToken.IsExpired)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.InvalidToken,
                ApiStatusCode.Unauthorized);
        }

        if (!refreshToken.User.IsActive)
        {
            return ResponseFactory.Failure<AuthResponseDto>(
                ApiMessages.AccountInactive,
                ApiStatusCode.Forbidden);
        }

        // Revoke old refresh token
        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = AppDateTime.Now;

        await _context.SaveChangesAsync();
        //await RevokeAllRefreshTokensAsync(refreshToken.UserId);
        // Generate new Access Token & Refresh Token
        var response = await GenerateAuthResponseAsync(refreshToken.User, refreshToken.DeviceId, refreshToken.DeviceName);

        return ResponseFactory.Success(
            response,
            ApiMessages.RefreshTokenGenerated);
    }

    public async Task<ApiResponse<object>> LogoutAsync(LogoutRequestDto request)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x =>
                x.Token == request.RefreshToken &&
                !x.IsRevoked);

        if (token == null)
        {
            return ResponseFactory.Failure<object>(
                ApiMessages.InvalidToken,
                ApiStatusCode.BadRequest);
        }

        token.IsRevoked = true;
        token.RevokedAt = AppDateTime.Now;

        await _context.SaveChangesAsync();

        return ResponseFactory.Success<object>(
             null,
             ApiMessages.LogoutSuccess);
    }

    #region Private Methods

    private async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Include(x => x.Customer)
            .Include(x => x.Partner)
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
    private async Task SaveRefreshTokenAsync(
    User user,
    string refreshToken,
    string? deviceId,
    string? deviceName)
    {
        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            var existingTokens = await _context.RefreshTokens
                .Where(x =>
                    x.UserId == user.Id &&
                    x.DeviceId == deviceId &&
                    !x.IsRevoked)
                .ToListAsync();

            foreach (var item in existingTokens)
            {
                item.IsRevoked = true;
                item.RevokedAt = AppDateTime.Now;
            }
        }

        var token = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = AppDateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            DeviceId = deviceId,
            DeviceName = deviceName
        };

        await _context.RefreshTokens.AddAsync(token);

        // One database call
        await _context.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(
     User user,
     string? deviceId,
     string? deviceName)
    {
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        await SaveRefreshTokenAsync(
            user,
            refreshToken,
            deviceId,
            deviceName);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name ?? string.Empty,
            IsNewUser = false,
            IsProfileCompleted = user.Customer != null || user.Partner != null,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RegistrationToken = null,
            ExpiresAt = AppDateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            RefreshTokenExpiresAt = AppDateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };
    }

    private async Task<Role?> GetRoleAsync(int roleId)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == roleId);
    }


    //private async Task RevokeAllRefreshTokensAsync(int userId)
    //{
    //    var refreshTokens = await _context.RefreshTokens
    //        .Where(x => x.UserId == userId && !x.IsRevoked)
    //        .ToListAsync();

    //    foreach (var token in refreshTokens)
    //    {
    //        token.IsRevoked = true;
    //        token.RevokedAt = AppDateTime.Now;
    //    }

    //    await _context.SaveChangesAsync();
    //}
    #endregion
}