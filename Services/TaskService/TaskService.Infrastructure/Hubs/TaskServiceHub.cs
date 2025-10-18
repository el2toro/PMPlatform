using Core.Services;
using Microsoft.AspNetCore.SignalR;
using TaskService.Application.Dtos;
using TaskService.Application.Interfaces;

namespace TaskService.Infrastructure.Hubs;

public class TaskServiceHub(IHubContext<TaskServiceHub> hubContext,
    TenantAwareContextService tenantAwareContextService)
    : Hub, ITaskServiceHub
{
    private readonly IHubContext<TaskServiceHub> _hubContext = hubContext;
    public async Task SendCreatedTask(TaskItemDto task)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveCreatedTask", task);
    }

    public async Task SendTaskAssignee(TaskItemDto task)
    {
        await _hubContext.Clients.User(tenantAwareContextService.ToString()!).SendAsync("ReceiveTaskAssignee", task);
    }

    public async Task SendTaskStatus(Guid taskId, TaskItemStatus status)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveTaskStatus", taskId, status);
    }

    public async Task SendUpdatedTask(TaskItemDto task)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdatedTask", task);
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected user: {Context.User?.FindFirst("sub")?.Value}");
        return base.OnConnectedAsync();
    }
}
