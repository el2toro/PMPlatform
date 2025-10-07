namespace TaskService.Application.Interfaces;

public interface ITaskServiceHub
{
    Task SendCreatedTask(TaskItemDto task);
    Task SendUpdatedTask(TaskItemDto task);
    Task SendTaskAssigneChanged(Guid taskId, Guid assignedTo);
    Task SendTaskStatusChanged(Guid taskId, int status);
}
