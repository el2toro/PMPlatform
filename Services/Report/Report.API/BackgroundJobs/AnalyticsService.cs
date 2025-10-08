namespace Report.API.BackgroundJobs;

public static class AnalyticsService
{
    public static Task RunAnalytics()
    {
        Console.WriteLine($"Analytics running at; {DateTimeOffset.UtcNow}");
        return Task.CompletedTask;
    }
}
