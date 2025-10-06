using Microsoft.AspNetCore.SignalR;
namespace Project.API.Hubs;

public interface IProjectHub
{
    Task SendProject(ProjectDto project);
    Task SendUpdatedProject(ProjectDto project);
}

public class ProjectHub(IHubContext<ProjectHub> hubContext) : Hub, IProjectHub
{
    private readonly IHubContext<ProjectHub> _hubContext = hubContext;
    public async Task SendProject(ProjectDto project)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveProjects", project);
    }

    public async Task SendUpdatedProject(ProjectDto project)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdatedProject", project);
    }
}
