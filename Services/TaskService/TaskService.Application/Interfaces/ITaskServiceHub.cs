using TaskService.Domain.Enums;

namespace TaskService.Application.Interfaces;

public interface ITaskServiceHub
{
    Task SendCreatedTask(TaskItemDto task);
    Task SendUpdatedTask(TaskItemDto task);
    Task SendTaskAssignee(TaskItemDto task);
    Task SendTaskStatus(Guid taskId, TaskItemStatus status);
}
