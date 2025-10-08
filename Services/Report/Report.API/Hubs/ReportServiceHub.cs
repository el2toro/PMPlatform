using Microsoft.AspNetCore.SignalR;

namespace Report.API.Hubs;

public interface IReportServiceHub
{
    Task SendKpis();
}
public class ReportServiceHub(IHubContext<ReportServiceHub> hubContext) : Hub, IReportServiceHub
{

    private readonly IHubContext<ReportServiceHub> _hubContext = hubContext;
    public async Task SendKpis()
    {
        await _hubContext.Clients.Group("").SendAsync("ReceiveKpis", "kpis");
    }
}
