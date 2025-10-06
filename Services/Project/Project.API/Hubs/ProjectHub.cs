using Microsoft.AspNetCore.SignalR;
using Project.API.Dtos;
using Project.API.Models;

namespace Project.API.Hubs;

public interface IProjectHub
{
    Task SendMessage(ProjectDto project);
}

public class ProjectHub(IHubContext<ProjectHub> hubContext) : Hub, IProjectHub
{
    private readonly IHubContext<ProjectHub> _hubContext = hubContext;
    public async Task SendMessage(ProjectDto project)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", project);
    }
}
