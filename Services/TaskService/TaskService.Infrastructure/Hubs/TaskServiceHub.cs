using Microsoft.AspNetCore.SignalR;
using TaskService.Application.Dtos;
using TaskService.Application.Interfaces;

namespace TaskService.Infrastructure.Hubs;

public class TaskServiceHub(IHubContext<TaskServiceHub> hubContext) : Hub, ITaskServiceHub
{
    private readonly IHubContext<TaskServiceHub> _hubContext = hubContext;
    public async Task SendCreatedTask(TaskItemDto task)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveCreatedTask", task);
    }

    public async Task SendTaskAssigneChanged(Guid taskId, Guid assignedTo)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveTaskAssignee", taskId, assignedTo);
    }

    public async Task SendTaskStatusChanged(Guid taskId, int status)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveTaskStatus", taskId, status);
    }

    public async Task SendUpdatedTask(TaskItemDto task)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdatedTask", task);
    }
}
