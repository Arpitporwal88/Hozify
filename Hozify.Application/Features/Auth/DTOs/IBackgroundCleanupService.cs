public interface IBackgroundCleanupService
{
    Task CleanupAsync(CancellationToken cancellationToken);
}