using Report.Worker.Interfaces;

namespace Report.Worker.Services;

public class ReportService : IReportService
{
    public Task RunAnalyticsAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Analytics is running");
        return Task.CompletedTask;
    }
}
