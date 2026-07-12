using Hozify.Application.Features.Auth.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hozify.Infrastructure.BackgroundServices;

public class CleanupHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CleanupHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var cleanupService =
                scope.ServiceProvider.GetRequiredService<IBackgroundCleanupService>();

            await cleanupService.CleanupAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}