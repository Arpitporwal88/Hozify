using Hozify.Domain.Common;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Services;

public class BackgroundCleanupService : IBackgroundCleanupService
{
    private readonly HozifyDbContext _context;

    public BackgroundCleanupService(HozifyDbContext context)
    {
        _context = context;
    }

    public async Task CleanupAsync(CancellationToken cancellationToken)
    {
        var otpDate = AppDateTime.Now.AddDays(-7);

        var oldOtps = await _context.OtpVerifications
            .Where(x =>
                x.CreatedAt <= otpDate &&
                x.Status != OtpStatus.Pending)
            .ToListAsync(cancellationToken);

        if (oldOtps.Any())
            _context.OtpVerifications.RemoveRange(oldOtps);

        var refreshDate = AppDateTime.Now;

        var refreshTokens = await _context.RefreshTokens
            .Where(x =>
                x.ExpiresAt <= refreshDate ||
                (x.IsRevoked &&
                 x.RevokedAt != null &&
                 x.RevokedAt <= AppDateTime.Now.AddDays(-7)))
            .ToListAsync(cancellationToken);

        if (refreshTokens.Any())
            _context.RefreshTokens.RemoveRange(refreshTokens);

        await _context.SaveChangesAsync(cancellationToken);
    }
}