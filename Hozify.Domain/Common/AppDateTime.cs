namespace Hozify.Domain.Common;

public static class AppDateTime
{
    public static DateTime Now => DateTime.Now;

    public static DateOnly Today => DateOnly.FromDateTime(Now);

    public static TimeOnly CurrentTime => TimeOnly.FromDateTime(Now);
}