using Report.Worker.Interfaces;

namespace Report.Worker
{
    public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Run every 5 minutes
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Analytics running at: {time}", DateTimeOffset.Now);

                var scope = _serviceScopeFactory.CreateScope();
                var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

                await reportService.RunAnalyticsAsync(stoppingToken);
            }
        }
    }
}
