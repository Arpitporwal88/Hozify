using Hozify.Application.Common.Settings;
using Hozify.Application.Features.Auth.Interfaces;
using Hozify.Domain.Common;
using Hozify.Domain.Entities;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Hozify.Infrastructure.Services;

public class OtpService : IOtpService
{
    private readonly HozifyDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public OtpService(HozifyDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task SendOtpAsync(string phoneNumber)
    {
        // Check Cooldown
        var lastOtp = await _context.OtpVerifications
            .Where(x => x.PhoneNumber == phoneNumber)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (lastOtp != null)
        {
            var elapsedSeconds = (AppDateTime.Now - lastOtp.CreatedAt).TotalSeconds;

            if (elapsedSeconds < _jwtSettings.OtpCooldownSeconds)
            {
                var remainingSeconds = _jwtSettings.OtpCooldownSeconds - (int)elapsedSeconds;

                throw new InvalidOperationException(
                    $"Please wait {remainingSeconds} seconds before requesting another OTP.");
            }
        }

        // Rate Limit Check
        var rateLimitFrom = AppDateTime.Now.AddMinutes(-_jwtSettings.OtpRateLimitMinutes);

        var recentOtps = await _context.OtpVerifications
            .Where(x =>
                x.PhoneNumber == phoneNumber &&
                x.CreatedAt >= rateLimitFrom)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        if (recentOtps.Count >= _jwtSettings.OtpMaxRequests)
        {
            var oldestOtp = recentOtps.First();

            var nextAllowedTime = oldestOtp.CreatedAt.AddMinutes(_jwtSettings.OtpRateLimitMinutes);

            var remainingMinutes = (int)Math.Ceiling(
                (nextAllowedTime - AppDateTime.Now).TotalMinutes);

            var unit = remainingMinutes == 1 ? "minute" : "minutes";
            throw new InvalidOperationException(
                $"Maximum OTP limit reached. Please try again after {remainingMinutes} {unit}.");
        }

        // Mark all pending OTPs as Replaced
        var pendingOtps = await _context.OtpVerifications
            .Where(x =>
                x.PhoneNumber == phoneNumber &&
                x.Status == OtpStatus.Pending)
            .ToListAsync();

        foreach (var otp in pendingOtps)
        {
            otp.Status = OtpStatus.Replaced;
        }

        // Generate Secure OTP
        var otpCode = RandomNumberGenerator
            .GetInt32(100000, 1000000)
            .ToString();

        var otpVerification = new OtpVerification
        {
            PhoneNumber = phoneNumber,
            Otp = otpCode,
            ExpiryTime = AppDateTime.Now.AddMinutes(_jwtSettings.OtpExpirationMinutes),
            Status = OtpStatus.Pending,
            AttemptCount = 0
        };

        await _context.OtpVerifications.AddAsync(otpVerification);

        await _context.SaveChangesAsync();

        // TODO : Replace with SMS Provider
        Console.WriteLine("====================================");
        Console.WriteLine($"OTP for {phoneNumber} : {otpCode}");
        Console.WriteLine("====================================");
    }
    public async Task<OtpVerificationStatus> VerifyOtpAsync(
    string phoneNumber,
    string otp)
    {
        var otpRecord = await _context.OtpVerifications
            .Where(x =>
                x.PhoneNumber == phoneNumber &&
                 x.Status == OtpStatus.Pending)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (otpRecord == null)
            return OtpVerificationStatus.InvalidOtp;

        // OTP Expired
        if (otpRecord.ExpiryTime < AppDateTime.Now)
        {
            otpRecord.Status = OtpStatus.Expired;

            await _context.SaveChangesAsync();

            return OtpVerificationStatus.Expired;
        }

        otpRecord.AttemptCount++;


        // Max Attempts
        if (otpRecord.AttemptCount >= 3)
        {
            otpRecord.Status = OtpStatus.Expired;
            await _context.SaveChangesAsync();

            return OtpVerificationStatus.MaxAttemptsExceeded;
        }

        // Wrong OTP
        if (otpRecord.Otp != otp)
        {
            await _context.SaveChangesAsync();

            return OtpVerificationStatus.InvalidOtp;
        }

        otpRecord.Status = OtpStatus.Verified;

        await _context.SaveChangesAsync();

        return OtpVerificationStatus.Success;
    }
}