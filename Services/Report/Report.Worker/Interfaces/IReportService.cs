namespace Report.Worker.Interfaces;

public interface IReportService
{
    Task RunAnalyticsAsync(CancellationToken cancellationToken);
}
